using KoosMatlilWebsite.Services;
using KoosMatliWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class CalendarController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public CalendarController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _apiService.GetEventsAsync();

            return View(events);
        }
    }
}
