using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Entities.Entities;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.ViewComponents
{
    public class CurrentUserViewComponent : ViewComponent
    {
        private readonly UserManager<CustomIdentityUser> _userManager;

        public CurrentUserViewComponent(UserManager<CustomIdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public ViewViewComponentResult Invoke()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            return View(new CurrentUserViewModel
            {
                User = user,
            });
        }
    }
}
