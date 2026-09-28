using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class StaffApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
