using Database.Backend;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{table?}")]
        public IActionResult Index(string table = null)
        {
            var data = new List<T1>();

            if (table == "T1")
            {
                ViewData["Table"] = table;
                data = LocalDbHelper.ReadT1();
            }

            return View(data);
        }

        [HttpPost("{table?}")]
        public IActionResult Index([Bind("ID,Name")] FilterModel filterModel, string table = null)
        {
            var data = new List<T1>();

            if (table == "T1")
            {
                ViewData["Table"] = table;
                data = LocalDbHelper.ReadT1(filterModel);
            }

            ViewData["Filter"] = filterModel;

            return View(data);
        }

        [HttpPost]
        public IActionResult Insert([Bind("ID,Name")] T1 data)
        {
            var success = LocalDbHelper.Insert(data);

            if (success)
            {
                return RedirectToAction("Index", new { table = "T1" });
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public IActionResult Update([Bind("ID,Name")] T1 data)
        {
            var success = LocalDbHelper.Update(data);

            if (success)
            {
                return RedirectToAction("Index", new { table = "T1" });
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public IActionResult Delete([Bind("ID")] T1 data)
        {
            var success = LocalDbHelper.Delete(data);

            if (success)
            {
                return RedirectToAction("Index", new { table = "T1" });
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public IActionResult Restore([Bind("Path")] string path)
        {
            var success = LocalDbHelper.Restore(path);

            if (success)
            {
                return RedirectToAction("Index", new { table = "T1" });
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public ContentResult Backup()
        {
            var success = LocalDbHelper.Backup();

            if (success)
            {
                return Content("{\"success\":true}", "application/json");
            }

            return Content("{\"success\":false}", "application/json");
        }

        [HttpGet]
        public FileStreamResult Download()
        {
            return new FileStreamResult(LocalDbHelper.Download(), "application/x-sql")
            {
                FileDownloadName = "localdb.sql"
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
