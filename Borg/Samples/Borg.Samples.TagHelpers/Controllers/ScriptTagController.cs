using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Samples.TagHelpers.Controllers
{
    public class ScriptTagController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ScriptTagController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
