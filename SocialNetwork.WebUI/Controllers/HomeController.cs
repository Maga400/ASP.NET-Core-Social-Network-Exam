using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService)
        {
            _logger = logger;
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user;    

            return View();
        }

        public async Task<IActionResult> GetAllUsers() 
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var users = await _customIdentityUserService.GetAllAsync();
            var datas = users.Where(u => u.Id != user.Id).OrderByDescending(u => u.IsOnline).ToList();
            return Ok(datas);
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
