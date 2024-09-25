using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Business.Services.Abstracts;
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
        public HomeController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IFriendService friendService, IFriendRequestService friendRequestService)
        {
            _logger = logger;
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _friendService = friendService;
            _friendRequestService = friendRequestService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user;

            return View();
        }

        public async Task<IActionResult> GetAllFriends()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var requests = await _friendRequestService.GetAllAsync();
            var datas = await _customIdentityUserService.GetAllAsync();
            var myRequests = requests.Where(r => r.SenderId == user.Id);
            var friends = await _friendService.GetAllAsync();
            var myFriends = friends.Where(f => f.OwnId == user.Id || f.YourFriendId == user.Id);

            var friendUsers = datas
            .Where(u => myFriends.Any(f => f.OwnId == u.Id || f.YourFriendId == u.Id) && u.Id != user.Id)
            .Select(u => new CustomIdentityUser
            {
                Id = u.Id,
                IsOnline = u.IsOnline,
                UserName = u.UserName,
                Image = u.Image,
                Email = u.Email
            })
            .ToList();


            //foreach (var item in users)
            //{
            //    var request = myRequests.FirstOrDefault(r => r.ReceiverId == item.Id && r.Status == "Request");
            //    if (request != null)
            //    {
            //        item.HasRequestPending = true;
            //        //await _customIdentityUserService.UpdateAsync(item);
            //    }
            //}

            return Ok(friendUsers);
        }
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


            //foreach (var item in users)
            //{
            //    var request = myRequests.FirstOrDefault(r => r.ReceiverId == item.Id && r.Status == "Request");
            //    if (request != null)
            //    {
            //        item.HasRequestPending = true;
            //        //await _customIdentityUserService.UpdateAsync(item);
            //    }
            //}

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
           

            //foreach (var item in users)
            //{
            //    var request = myRequests.FirstOrDefault(r => r.ReceiverId == item.Id && r.Status == "Request");
            //    if (request != null)
            //    {
            //        item.HasRequestPending = true;
            //        //await _customIdentityUserService.UpdateAsync(item);
            //    }
            //}

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
                });

                await _userManager.UpdateAsync(receiverUser);

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
        public async Task<IActionResult> UnfollowUser(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var friends = await _friendService.GetAllAsync();
            var friend = friends.FirstOrDefault(f => f.YourFriendId == user.Id && f.OwnId == id || f.OwnId == user.Id && f.YourFriendId == id);
            if(friend != null) 
            {
                await _friendService.DeleteAsync(friend);
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
