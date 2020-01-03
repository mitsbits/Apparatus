using Borg.Framework.MVC;
using Borg.Platform.EF;
using Microsoft.AspNetCore.Mvc;

namespace Apparatus.System.Backoffice.Areas
{
    [Area("apparatus")]
    public class HomeController : BackofficeControllerBase
    {
   
        public HomeController()
        {

        }
        public ActionResult Index()
        {
           
            return View();
        }
    }
}