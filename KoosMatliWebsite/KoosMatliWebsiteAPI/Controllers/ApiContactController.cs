using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public ContactController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ContactMessage>> SubmitMessage(
            ContactMessage message)
        {
            message.Id = 0;
            message.SubmittedAt = DateTime.UtcNow;
            message.IsRead = false;

            _context.ContactMessages.Add(message);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Your message has been received."
            });
        }
    }
}
