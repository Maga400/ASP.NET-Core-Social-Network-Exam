﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Business.Services.Concretes;
using SocialNetwork.DataAccess.Data;
using SocialNetwork.Entities.Entities;

namespace SocialNetwork.WebUI.Controllers
{
    public class NewsFeedController : Controller
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

        public NewsFeedController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IFriendService friendService, IFriendRequestService friendRequestService, IChatService chatService, IMessageService messageService, SocialNetworkDbContext context, INotificationService notificationService, IPostService postService, ICommentService commentService)
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
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllPosts()
        {
            var allPosts = await _postService.GetAllAsync();
            var current = await _userManager.GetUserAsync(HttpContext.User);
            //var notifications = allNotifications.Where(r => r.ReceiverId == current.Id);
            //Task.Delay(1000);
            return Ok(new { posts = allPosts, currentId = current.Id, currentImage = current.Image });
        }

        public async Task<IActionResult> GetMyPosts()
        {
            var allPosts = await _postService.GetAllAsync();
            var current = await _userManager.GetUserAsync(HttpContext.User);

            var myPosts = allPosts.Where(p => p.SenderId == current.Id);

            return Ok(new { posts = myPosts, currentId = current.Id, currentImage = current.Image });
        }

        public async Task<IActionResult> SharePost(string text = "")
        {
            var sender = await _userManager.GetUserAsync(HttpContext.User);

            var notification = new Notification
            {
                Content = $"{sender.UserName} share a new post at {DateTime.Now.ToLongDateString()} + {text}",
                UserId = sender.Id,
                User = sender,
                Status = "Notification"
            };

            await _notificationService.AddAsync(notification);
            return Ok();

        }
    }
}
