using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Entities.Entities;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IFriendService _friendService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public FriendsController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IFriendService friendService, IFriendRequestService friendRequestService, IChatService chatService, IMessageService messageService)
        {
            _logger = logger;
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _friendService = friendService;
            _friendRequestService = friendRequestService;
            _chatService = chatService;
            _messageService = messageService;
        }
        // GET: FriendsController
        public ActionResult Index()
        {
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

            return Ok(friendUsers);
        }

        // GET: FriendsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FriendsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FriendsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FriendsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FriendsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
