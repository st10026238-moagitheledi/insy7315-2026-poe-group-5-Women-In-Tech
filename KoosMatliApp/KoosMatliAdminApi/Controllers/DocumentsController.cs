using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers
{
    public class DocumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
