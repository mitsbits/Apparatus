using Borg;
using Borg.Framework.SQLServer.Broadcast;
using Borg.Framework.Storage.Contracts;
using Borg.Framework.Dispatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestFileProviders.Models;
using Borg.Platform.EF;
using Borg.Platform.EF.Silos;
using Borg.Framework.DAL.Ordering;

namespace TestFileProviders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileDepot _depot;
        private readonly ISqlBroadcastBus _sqlBroadcastBus;

        public HomeController(ILogger<HomeController> logger, IFileDepot depot, ISqlBroadcastBus sqlBroadcastBus)
        {
            _logger = logger;
            _depot = depot;
            _sqlBroadcastBus = sqlBroadcastBus;
        }

        public async Task<IActionResult> Index()
        {
            await _sqlBroadcastBus.Publish("a queue", new AMessage { Id = 5, Message = "Hello there!" });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> DbContext()
        {
            using (var db = new PlatformDb())
            {
                var l = db.ReadRepo<Language, PlatformDb>();
                var s = await l.Find(x => true, SortBuilder.Get<Language>().Build());
            }
            return Ok();
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

    public class AMessage : INotification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}