using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public ContactController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            message.SubmittedAt = DateTime.UtcNow;

            _context.ContactMessages.Add(message);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Your message has been sent successfully."
            });
        }
    }
}