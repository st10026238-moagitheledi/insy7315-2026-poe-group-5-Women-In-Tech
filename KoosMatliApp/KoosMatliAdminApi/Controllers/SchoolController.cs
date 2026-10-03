using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers
{
    public class SchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
