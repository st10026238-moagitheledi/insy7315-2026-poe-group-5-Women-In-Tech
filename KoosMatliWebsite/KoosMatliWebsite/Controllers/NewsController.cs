using Microsoft.AspNetCore.Mvc;
using KoosMatliWebsite.Models;
using KoosMatlilWebsite.Services;

namespace KoosMatliWebsite.Controllers
{
    public class NewsController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public NewsController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _apiService.GetNewsAsync();

            return View(news);
        }

        public async Task<IActionResult> Details(int id)
        {
            var news = await _apiService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }
    }
}