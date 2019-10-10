using Borg;
using Borg.Framework.Storage.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestFileProviders.Models;

namespace TestFileProviders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileDepot _depot;

        public HomeController(ILogger<HomeController> logger, IFileDepot depot)
        {
            _logger = logger;
            _depot = depot;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Upload()
        {
            var path = string.Empty;
            var prev = TempData["directory"];
            if (prev != null)
            {
                path = prev.ToString();
            }

            var data = _depot.GetDirectoryContents(path);

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadViewModel model)
        {
            var file = model.Files.FirstOrDefault();
            var name = file.FileName.MakeValidFileName();
            await _depot.Save($"{model.Path}/{name }", file.OpenReadStream());
            TempData["directory"] = model.Path;
            return RedirectToAction(nameof(Upload));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class UploadViewModel
        {
            public List<IFormFile> Files { get; set; }
            public string Path { get; set; }
        }
    }
}