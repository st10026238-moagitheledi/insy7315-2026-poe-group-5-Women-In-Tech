using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public EventsController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolEvent>>> GetEvents()
        {
            return await _context.SchoolEvents
                .Where(e => e.IsPublished)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolEvent>> GetEvent(int id)
        {
            var schoolEvent =
                await _context.SchoolEvents.FindAsync(id);

            if (schoolEvent == null || !schoolEvent.IsPublished)
            {
                return NotFound();
            }

            return schoolEvent;
        }

        [HttpPost]
        public async Task<ActionResult<SchoolEvent>> CreateEvent(
            SchoolEvent schoolEvent)
        {
            schoolEvent.Id = 0;

            _context.SchoolEvents.Add(schoolEvent);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = schoolEvent.Id },
                schoolEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            SchoolEvent schoolEvent)
        {
            if (id != schoolEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(schoolEvent).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var schoolEvent =
                await _context.SchoolEvents.FindAsync(id);

            if (schoolEvent == null)
            {
                return NotFound();
            }

            _context.SchoolEvents.Remove(schoolEvent);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
