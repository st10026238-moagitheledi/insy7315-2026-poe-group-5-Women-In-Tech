using KoosMatlilWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class AboutController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public AboutController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var information =
                await _apiService.GetSchoolInformationAsync();

            if (information == null)
            {
                return NotFound();
            }

            return View(information);
        }
    }
}
