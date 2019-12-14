using Microsoft.AspNetCore.Mvc;

namespace Apparatus.System.Backoffice.Areas
{
    [Area("apparatus")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}