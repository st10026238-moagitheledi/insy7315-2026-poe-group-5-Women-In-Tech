using KoosMatlilWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class GalleryController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public GalleryController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var gallery = await _apiService.GetGalleryAsync();

            return View(gallery);
        }
    }
}
