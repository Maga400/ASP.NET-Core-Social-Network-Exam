using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebUI.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
