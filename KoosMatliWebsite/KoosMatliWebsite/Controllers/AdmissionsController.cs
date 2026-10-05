using KoosMatlilWebsite.Services;
using KoosMatliWebsite.DTOs;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var admissions = await _apiService.GetAdmissionsAsync();

            return View(admissions);
        }

        [HttpGet]
        public IActionResult Apply()
        {
            return View(new ApplicationDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(
            ApplicationDto application)
        {
            if (!ModelState.IsValid)
            {
                return View(application);
            }

            var success =
                await _apiService.SubmitApplicationAsync(application);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "We could not submit your application. Please try again."
                );

                return View(application);
            }

            return RedirectToAction(
                nameof(ApplicationSubmitted));
        }

        [HttpGet]
        public IActionResult ApplicationSubmitted()
        {
            return View();
        }
    }
}