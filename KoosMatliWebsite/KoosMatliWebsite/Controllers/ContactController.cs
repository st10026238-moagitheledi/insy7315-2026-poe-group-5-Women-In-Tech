using KoosMatlilWebsite.Services;
using KoosMatliWebsite.DTOs;
using KoosMatliWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class ContactController : Controller
    {
        private readonly WebsiteApiService _apiService;

        public ContactController(WebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var schoolInformation =
                await _apiService.GetSchoolInformationAsync();

            var viewModel = new ContactViewModel
            {
                SchoolInformation = schoolInformation,
                ContactMessage = new Models.ContactMessage()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(
            ContactViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.SchoolInformation =
                    await _apiService.GetSchoolInformationAsync();

                return View("Index", viewModel);
            }

            var contactMessageDto = new ContactMessageDto
            {
                Name = viewModel.ContactMessage.Name,
                Email = viewModel.ContactMessage.Email,
                Subject = viewModel.ContactMessage.Subject,
                Message = viewModel.ContactMessage.Message
            };

            var success =
                await _apiService.SendContactMessageAsync(
                    contactMessageDto);

            if (!success)
            {
                ModelState.AddModelError(
                    "",
                    "We could not send your message. Please try again."
                );

                viewModel.SchoolInformation =
                    await _apiService.GetSchoolInformationAsync();

                return View("Index", viewModel);
            }

            TempData["SuccessMessage"] =
                "Your message has been sent successfully. Thank you for contacting us!";

            return RedirectToAction(nameof(Index));
        }
    }
}