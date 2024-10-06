using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Business.Services.Concretes;
using SocialNetwork.DataAccess.Data;
using SocialNetwork.Entities.Entities;
using SocialNetwork.WebUI.Models;
using System.Diagnostics;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IFriendService _friendService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly SocialNetworkDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikedPostService _likedPostService;
        private readonly ILikedCommentService _likedCommentService;
        private readonly IMyNotificationService _myNotificationService;

        public HomeController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IFriendService friendService, IFriendRequestService friendRequestService, IChatService chatService, IMessageService messageService, SocialNetworkDbContext context, INotificationService notificationService, IPostService postService, ICommentService commentService, ILikedPostService likedPostService, ILikedCommentService likedCommentService, IMyNotificationService myNotificationService)
        {
            _logger = logger;
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _friendService = friendService;
            _friendRequestService = friendRequestService;
            _chatService = chatService;
            _messageService = messageService;
            _context = context;
            _notificationService = notificationService;
            _postService = postService;
            _commentService = commentService;
            _likedPostService = likedPostService;
            _likedCommentService = likedCommentService;
            _myNotificationService = myNotificationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user;

            return View();
        }

        //public async Task<IActionResult> GetAllFriends()
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);
        //    var requests = await _friendRequestService.GetAllAsync();
        //    var datas = await _customIdentityUserService.GetAllAsync();
        //    var myRequests = requests.Where(r => r.SenderId == user.Id);
        //    var friends = await _friendService.GetAllAsync();
        //    var myFriends = friends.Where(f => f.OwnId == user.Id || f.YourFriendId == user.Id);

        //    var friendUsers = datas
        //    .Where(u => myFriends.Any(f => f.OwnId == u.Id || f.YourFriendId == u.Id) && u.Id != user.Id)
        //    .Select(u => new CustomIdentityUser
        //    {
        //        Id = u.Id,
        //        IsOnline = u.IsOnline,
        //        UserName = u.UserName,
        //        Image = u.Image,
        //        Email = u.Email
        //    })
        //    .ToList();

        //    return Ok(friendUsers);
        //}
        public async Task<IActionResult> GetAllUsersForLayout()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var requests = await _friendRequestService.GetAllAsync();
            var datas = await _customIdentityUserService.GetAllAsync();
            var myRequests = requests.Where(r => r.SenderId == user.Id);
            var friends = await _friendService.GetAllAsync();
            var myFriends = friends.Where(f => f.OwnId == user.Id || f.YourFriendId == user.Id);
            var users = datas
                .Where(u => u.Id != user.Id)
                .OrderByDescending(u => u.IsOnline)
                .Select(u => new CustomIdentityUser
                {
                    Id = u.Id,
                    HasRequestPending = (myRequests.FirstOrDefault(r => r.ReceiverId == u.Id && r.Status == "Request") != null),
                    IsFriend = myFriends.FirstOrDefault(f => f.OwnId == u.Id || f.YourFriendId == u.Id) != null,
                    IsOnline = u.IsOnline,
                    UserName = u.UserName,
                    Image = u.Image,
                    Email = u.Email,

                }).ToList();

            return Ok(users);
        }
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var requests = await _friendRequestService.GetAllAsync();
            var datas = await _customIdentityUserService.GetAllAsync();
            var myRequests = requests.Where(r => r.SenderId == user.Id);
            var friends = await _friendService.GetAllAsync();
            var myFriends = friends.Where(f => f.OwnId == user.Id || f.YourFriendId == user.Id);
            var users = datas
                .Where(u => u.Id != user.Id)
                .OrderByDescending(u => u.IsOnline)
                .Select(u => new CustomIdentityUser
                {
                    Id = u.Id,
                    HasRequestPending = (myRequests.FirstOrDefault(r => r.ReceiverId == u.Id && r.Status == "Request") != null),
                    IsFriend = myFriends.FirstOrDefault(f => f.OwnId == u.Id || f.YourFriendId == u.Id) != null,
                    IsOnline = u.IsOnline,
                    UserName = u.UserName,
                    Image = u.Image,
                    Email = u.Email,

                }).Where(u => u.IsFriend == false).ToList();

            return Ok(users);
        }

        public async Task<IActionResult> SendFollow(string id)
        {
            var sender = await _userManager.GetUserAsync(HttpContext.User);
            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (receiverUser != null)
            {
                await _friendRequestService.AddAsync(new FriendRequest
                {
                    Content = $"{sender.UserName} sent friend request at {DateTime.Now.ToLongDateString()}",
                    SenderId = sender.Id,
                    Sender = sender,
                    ReceiverId = id,
                    Status = "Request"
                });

                return Ok();

            }

            return BadRequest();
        }



        public async Task<IActionResult> SendComment(int id, string message,string senderId)
        {
            var post = await _postService.GetByIdAsync(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (post != null)
            {
                var comment = new Comment
                {
                    PostId = post.Id,
                    Post = post,
                    Content = message,
                    WritingDate = DateTime.Now,
                    Sender = user,
                    SenderId = user.Id,
                };

                post.CommentCount += 1;

                await _postService.UpdateAsync(post);
                await _commentService.AddAsync(comment);
            }

            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == senderId);
            if (receiverUser != null)
            {
                await _myNotificationService.AddAsync(new MyNotification
                {
                    Content = $"{user.UserName} commented your post {DateTime.Now.ToLongDateString()}",
                    SenderId = user.Id,
                    Sender = user,
                    ReceiverId = receiverUser.Id,
                    Status = "Notification"
                });

            }

            return Ok();
        }
        public async Task<IActionResult> SendLike(int id,string currentId)
        {
            var post = await _postService.GetByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var likedPosts = await _likedPostService.GetAllAsync();
            var message = "";
            if (post != null)
            {
                var likedPost = likedPosts.FirstOrDefault(l => l.UserId == currentUser.Id && l.PostId == post.Id);

                if(likedPost == null) 
                {
                    message = "liked";
                    post.LikeCount += 1;
                    await _postService.UpdateAsync(post);

                    var newLikedPost = new LikedPost()
                    {
                        PostId =post.Id,
                        Post = post,
                        UserId = currentUser.Id,
                        User = currentUser
                    };

                    await _likedPostService.AddAsync(newLikedPost);
                }
                else
                {
                    message = "disliked";
                    post.LikeCount -= 1;
                    await _postService.UpdateAsync(post);
                    await _likedPostService.DeleteAsync(likedPost);
                }

                
            }

            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentId);
            if (receiverUser != null)
            {
                await _myNotificationService.AddAsync(new MyNotification
                {
                    Content = $"{currentUser.UserName} {message} your post {DateTime.Now.ToLongDateString()}",
                    SenderId = currentUser.Id,
                    Sender = currentUser,
                    ReceiverId = receiverUser.Id,
                    Status = "Notification"
                });

            }

            return Ok();
        }

        public async Task<IActionResult> SendCommentLike(int id,string senderId)
        {
            var comment = await _commentService.GetByIdAsync(id);
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var likedComments = await _likedCommentService.GetAllAsync();
            string message = "";
            if (comment != null)
            {
                var likedComment = likedComments.FirstOrDefault(l => l.UserId == currentUser.Id && l.CommentId == comment.Id);

                if (likedComment == null)
                {
                    message = "liked";

                    comment.LikeCount += 1;
                    await _commentService.UpdateAsync(comment);

                    var newLikedComment = new LikedComment()
                    {
                        CommentId = comment.Id,
                        Comment = comment,
                        UserId = currentUser.Id,
                        User = currentUser
                    };

                    await _likedCommentService.AddAsync(newLikedComment);
                }
                else
                {
                    message = "disliked";

                    comment.LikeCount -= 1;
                    await _commentService.UpdateAsync(comment);
                    await _likedCommentService.DeleteAsync(likedComment);
                }


            }

            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == senderId);
            if (receiverUser != null && receiverUser.Id != currentUser.Id)
            {
                await _myNotificationService.AddAsync(new MyNotification
                {
                    Content = $"{currentUser.UserName} {message} your post {DateTime.Now.ToLongDateString()}",
                    SenderId = currentUser.Id,
                    Sender = currentUser,
                    ReceiverId = receiverUser.Id,
                    Status = "Notification"
                });

            }

            return Ok();
        }
        public async Task<IActionResult> SharePost(string text)
        {
            var sender = await _userManager.GetUserAsync(HttpContext.User);

            var notification = new Notification
            {
                Content = $"{sender.UserName} share a new post at {DateTime.Now.ToLongDateString()}",
                UserId = sender.Id,
                User = sender,
                Status = "Notification",
                Date = DateTime.Now,
                
            };

            var post = new Post
            {
                Text = text,
                ShareDate = DateTime.Now,
                SenderId = sender.Id,
                Sender = sender,
                ImagePath = "",
                LikeCount = 0,
                CommentCount = 0,
                Comments = new List<Comment>()
            };

            await _notificationService.AddAsync(notification);
            await _postService.AddAsync(post);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> TakeRequest(string id)
        {
            var current = await _userManager.GetUserAsync(HttpContext.User);
            var friendRequests = await _friendRequestService.GetAllAsync();
            var request = friendRequests.FirstOrDefault(r => r.SenderId == current.Id && r.ReceiverId == id);

            if (request == null) return NotFound();
            await _friendRequestService.DeleteAsync(request);

            return Ok();
        }

        public async Task<IActionResult> GetAllRequests()
        {
            var current = await _userManager.GetUserAsync(HttpContext.User);
            var friendRequests = await _friendRequestService.GetAllAsync();
            var requests = friendRequests.Where(r => r.ReceiverId == current.Id);

            return Ok(requests);
        }

        public async Task<IActionResult> GetMyNotifications()
        {
            var current = await _userManager.GetUserAsync(HttpContext.User);
            var myNotifications = await _myNotificationService.GetAllAsync();
            var notifications = myNotifications.Where(r => r.ReceiverId == current.Id);

            return Ok(notifications);
        }

        public async Task<IActionResult> GetAllNotification()
        {
            var allNotifications = await _notificationService.GetAllAsync();
            var current = await _userManager.GetUserAsync(HttpContext.User);
            var allFriends = await _friendService.GetAllAsync();
            var friendNotifications = allNotifications.Where(n => allFriends.All(f => f.OwnId == n.UserId)).ToList();

            var friends = allFriends
            .Where(f => f.OwnId == current.Id || f.YourFriendId == current.Id)
            .ToList();

            // Arkadaşlara ait olan bildirimleri getir
            var notifications = allNotifications
            .Where(n =>
                friends.Any(f =>
                    (f.OwnId == current.Id && n.UserId == f.YourFriendId && n.Date > f.FriendDate) ||
                    (f.YourFriendId == current.Id && n.UserId == f.OwnId && n.Date > f.FriendDate)
                )
            )
            .ToList();
            //var friendUsers = datas
            //.Where(u => myFriends.Any(f => f.OwnId == u.Id || f.YourFriendId == u.Id) && u.Id != user.Id)
            //.Select(u => new CustomIdentityUser
            //{
            //    Id = u.Id,
            //    IsOnline = u.IsOnline,
            //    UserName = u.UserName,
            //    Image = u.Image,
            //    Email = u.Email
            //})
            //.ToList();
            //var notifications = allNotifications.Where(r => r.ReceiverId == current.Id);
            //Task.Delay(1000);

            return Ok(new { notifications = notifications, currentId = current.Id });
        }

        //public async Task<IActionResult> GetAllPosts()
        //{
        //    var allPosts = await _postService.GetAllAsync();
        //    var current = await _userManager.GetUserAsync(HttpContext.User);
        //    //var notifications = allNotifications.Where(r => r.ReceiverId == current.Id);
        //    //Task.Delay(1000);
        //    return Ok(new { posts = allPosts, currentId = current.Id, currentImage = current.Image });
        //}

        [HttpGet]
        public async Task<IActionResult> DeclineRequest(int id, string senderid)
        {
            try
            {
                var current = await _userManager.GetUserAsync(HttpContext.User);
                var friendRequests = await _friendRequestService.GetAllAsync();
                var request = friendRequests.FirstOrDefault(f => f.Id == id);
                await _friendRequestService.DeleteAsync(request);

                await _friendRequestService.AddAsync(new FriendRequest
                {
                    Content = $"{current.UserName} declined your friend request at {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString}",
                    SenderId = current.Id,
                    Sender = current,
                    ReceiverId = senderid,
                    Status = "Notification"
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AcceptRequest(string userId, string senderId, int requestId)
        {
            var receiverUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var sender = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == senderId);

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == receiverUser.Id || c.ReceiverId == user.Id && c.SenderId == receiverUser.Id);

            if (receiverUser != null)
            {
                await _friendRequestService.AddAsync(new FriendRequest
                {
                    Content = $"{sender.UserName} accepted friend request at ${DateTime.Now.ToLongDateString()} ${DateTime.Now.ToShortTimeString()}",
                    SenderId = senderId,
                    ReceiverId = receiverUser.Id,
                    Sender = sender,
                    Status = "Notification"
                });

                var friendRequests = await _friendRequestService.GetAllAsync();
                var request = friendRequests.FirstOrDefault(r => r.Id == requestId);
                await _friendRequestService.DeleteAsync(request);

                await _friendService.AddAsync(new Friend
                {
                    OwnId = sender.Id,
                    YourFriendId = receiverUser.Id,
                    FriendDate = DateTime.Now,
                });

                await _userManager.UpdateAsync(receiverUser);

                if (chat == null)
                {
                    chat = new Chat
                    {
                        ReceiverId = receiverUser.Id,
                        SenderId = user.Id,
                        Messages = new List<Message>()
                    };

                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            try
            {
                var friendRequests = await _friendRequestService.GetAllAsync();
                var request = friendRequests.FirstOrDefault();
                if (request == null) return NotFound();

                await _friendRequestService.DeleteAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var myNotifications = await _myNotificationService.GetAllAsync();
                var notification = myNotifications.FirstOrDefault(n => n.Id == id);
                if (notification == null) return NotFound();

                await _myNotificationService.DeleteAsync(notification);
                return Ok();
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> UnfollowUser(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var friends = await _friendService.GetAllAsync();
            var friend = friends.FirstOrDefault(f => f.YourFriendId == user.Id && f.OwnId == id || f.OwnId == user.Id && f.YourFriendId == id);
            var chats = await _chatService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.ReceiverId == friend.YourFriendId && c.SenderId == friend.OwnId || c.ReceiverId == friend.OwnId && c.SenderId == friend.YourFriendId);


            if (friend != null)
            {
                await _friendService.DeleteAsync(friend);
                await _chatService.DeleteAsync(chat);
                return Ok();
            }

            return NotFound();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
