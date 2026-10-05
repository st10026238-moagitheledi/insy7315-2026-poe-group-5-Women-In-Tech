using Microsoft.AspNetCore.Mvc;
using KoosMatlilWebsite.Services;

namespace KoosMatliWebsite.Controllers
{
    public class StaffController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public StaffController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var staff = await _apiService.GetStaffAsync();

            return View(staff);
        }
    }
}
