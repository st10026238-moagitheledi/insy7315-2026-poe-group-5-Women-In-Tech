using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
