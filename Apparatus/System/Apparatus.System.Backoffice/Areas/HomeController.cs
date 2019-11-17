using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

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
