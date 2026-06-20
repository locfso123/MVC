using Microsoft.AspNetCore.Mvc;
using mvc01.Models;
using System.Diagnostics;

namespace mvc01.Controllers
{
    public class HomeController : Controller
    {
        public string HiHome() => "Xin chao cac ban, toi la HiHome";

        public IActionResult Index()
        {
            return View();
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
