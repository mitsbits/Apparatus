using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apparatus.Application.Server
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new OkResult();
        }
    }


}
