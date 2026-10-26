using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;

namespace KoosMatliAdminApi.Controllers;

[ApiController]
[Route("api/admin/school")]
public class SchoolController : ControllerBase
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

    [HttpGet("details")]
    public async Task<ActionResult<SchoolDetails>> GetDetails()
    {
        var details = await _context.SchoolDetails.FirstOrDefaultAsync();

        if (details == null)
            return NotFound();

        return Ok(details);
    }

    // PUT: api/admin/school/details
    [HttpPut("details")]
    public async Task<ActionResult<SchoolDetails>> UpdateDetails(
        [FromBody] SchoolDetails updatedDetails)
    {
        var details = await _context.SchoolDetails.FirstOrDefaultAsync();

        if (details == null)
            return NotFound();

        details.SchoolName = updatedDetails.SchoolName;
        details.Address = updatedDetails.Address;
        details.PhoneNumber = updatedDetails.PhoneNumber;
        details.Email = updatedDetails.Email;
        details.PrincipalName = updatedDetails.PrincipalName;

        await _context.SaveChangesAsync();

        return Ok(details);
    }

    // GET: api/admin/school/dates
    [HttpGet("dates")]
    public async Task<ActionResult<IEnumerable<SchoolDate>>> GetDates()
    {
        var dates = await _context.SchoolDates
            .OrderBy(d => d.Date)
            .ToListAsync();

        return Ok(dates);
    }

    // POST: api/admin/school/dates
    [HttpPost("dates")]
    public async Task<ActionResult<SchoolDate>> AddDate(
        [FromBody] SchoolDate schoolDate)
    {
        if (string.IsNullOrWhiteSpace(schoolDate.Title))
            return BadRequest("A title is required.");

        if (schoolDate.Date == default)
            return BadRequest("A valid date is required.");

        schoolDate.Title = schoolDate.Title.Trim();
        schoolDate.Description =
            schoolDate.Description?.Trim() ?? string.Empty;

        _context.SchoolDates.Add(schoolDate);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetDates),
            new { id = schoolDate.Id },
            schoolDate);
    }

    // GET: api/admin/school/calendar
    [HttpGet("calendar")]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendar()
    {
        var events = await _context.Set<CalendarEvent>()
            .OrderBy(e => e.Start)
            .ToListAsync();

        return Ok(events);
    }
}