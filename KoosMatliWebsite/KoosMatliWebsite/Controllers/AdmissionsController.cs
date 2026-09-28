using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class AdmissionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Apply()
        {
            return View();
        }
    }
}
