using KoosMatlilWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class AdmissionsController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public AdmissionsController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var admissions = await _apiService.GetAdmissionsAsync();

            return View(admissions);
        }
    }
}
