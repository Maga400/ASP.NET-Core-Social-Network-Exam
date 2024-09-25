using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
