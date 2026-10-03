using Microsoft.AspNetCore.Mvc;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/school")]
    public class SchoolController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SchoolController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/school/details
        [HttpGet("details")]
        public async Task<ActionResult<SchoolDetails>> GetDetails()
        {
            var details = await _context.SchoolDetails.FirstOrDefaultAsync();
            if (details == null) return NotFound();
            return Ok(details);
        }

        // GET: api/admin/school/dates
        [HttpGet("dates")]
        public async Task<ActionResult<IEnumerable<SchoolDate>>> GetDates()
        {
            var dates = await _context.SchoolDates.ToListAsync();
            return Ok(dates);
        }

        // GET: api/admin/school/calendar
        [HttpGet("calendar")]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendar()
        {
            var events = await _context.Set<CalendarEvent>().ToListAsync();
            return Ok(events);
        }
    }
}
