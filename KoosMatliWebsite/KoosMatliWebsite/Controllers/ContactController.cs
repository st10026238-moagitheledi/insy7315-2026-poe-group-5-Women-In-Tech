using KoosMatliWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(ContactMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // We will connect this to the database later.

            TempData["SuccessMessage"] = "Thank you! Your message has been received.";

            return RedirectToAction(nameof(Index));
        }
    }
}
