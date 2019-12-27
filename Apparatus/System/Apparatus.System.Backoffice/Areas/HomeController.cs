using Borg.Platform.EF;
using Microsoft.AspNetCore.Mvc;

namespace Apparatus.System.Backoffice.Areas
{
    [Area("apparatus")]
    public class HomeController : Controller
    {
        private readonly PlatformDb platformDb;
        public HomeController(PlatformDb platformDb)
        {
            this.platformDb = platformDb;
        }
        public ActionResult Index()
        {
            var r = platformDb.Pages;
            return View();
        }
    }
}