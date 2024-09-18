using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebUI.Controllers
{
    public class NewsFeed : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
