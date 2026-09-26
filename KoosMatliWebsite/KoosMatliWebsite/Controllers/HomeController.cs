using KoosMatliWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KoosMatliWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
