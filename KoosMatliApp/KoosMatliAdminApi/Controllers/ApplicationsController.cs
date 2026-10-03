using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
