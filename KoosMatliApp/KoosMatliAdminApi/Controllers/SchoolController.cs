using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] //(Microsoft Learn, 2026a).
[ApiController]
[Route("api/admin/school")]
public class SchoolController : ControllerBase
{
    private readonly FirestoreDataStore _store;
    public SchoolController(FirestoreDataStore store) => _store = store;

    [HttpGet("details")]
    public async Task<ActionResult<SchoolDetails>> GetDetails() //Microsoft Learn, 2026b).
    {
        var details = await _store.GetFirstAsync<SchoolDetails>("schoolDetails"); //(Google Cloud, 2026).
        return details is null ? NotFound() : Ok(details);
    }

    [HttpPut("details")]
    public async Task<ActionResult<SchoolDetails>> UpdateDetails([FromBody] SchoolDetails updatedDetails)
    {
        if (string.IsNullOrWhiteSpace(updatedDetails.SchoolName))
            return BadRequest("A school name is required.");

        var details = await _store.GetFirstAsync<SchoolDetails>("schoolDetails");
        if (details is null) {
            updatedDetails.Id = 1;
            await _store.SaveAsync("schoolDetails", updatedDetails); //(Google Cloud, 2026).
            return Ok(updatedDetails);
        }

        details.SchoolName = updatedDetails.SchoolName.Trim();
        details.Address = updatedDetails.Address?.Trim() ?? string.Empty;
        details.PhoneNumber = updatedDetails.PhoneNumber?.Trim() ?? string.Empty;
        details.Email = updatedDetails.Email?.Trim() ?? string.Empty;
        details.PrincipalName = updatedDetails.PrincipalName?.Trim() ?? string.Empty;
        await _store.SaveAsync("schoolDetails", details);
        return Ok(details);
    }

    [HttpGet("dates")]
    public async Task<ActionResult<IEnumerable<SchoolDate>>> GetDates()
    {
        var dates = await _store.GetAllAsync<SchoolDate>("schoolDates");
        return Ok(dates.OrderBy(d => d.Date));
    }

    [HttpPost("dates")]
    public async Task<ActionResult<SchoolDate>> AddDate([FromBody] SchoolDate schoolDate)
    {
        if (string.IsNullOrWhiteSpace(schoolDate.Title)) return BadRequest("A title is required.");
        if (schoolDate.Date == default) return BadRequest("A valid date is required.");
        schoolDate.Id = 0;
        schoolDate.Title = schoolDate.Title.Trim();
        schoolDate.Description = schoolDate.Description?.Trim() ?? string.Empty;
        await _store.AddAsync("schoolDates", schoolDate); //(Google Cloud, 2026).
        return CreatedAtAction(nameof(GetDates), new { id = schoolDate.Id }, schoolDate);
    }

    [HttpGet("calendar")]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendar()
    {
        var events = await _store.GetAllAsync<CalendarEvent>("calendarEvents");
        return Ok(events.OrderBy(e => e.Start));
    }

    [HttpPost("calendar")]
    public async Task<ActionResult<CalendarEvent>> AddCalendarEvent([FromBody] CalendarEvent calendarEvent)
    {
        if (string.IsNullOrWhiteSpace(calendarEvent.Title)) return BadRequest("An event title is required.");
        if (calendarEvent.Start == default) return BadRequest("A valid event start date and time is required.");
        if (calendarEvent.End.HasValue && calendarEvent.End.Value < calendarEvent.Start)
            return BadRequest("The event end must be after its start.");

        calendarEvent.Id = 0;
        calendarEvent.Title = calendarEvent.Title.Trim();
        calendarEvent.Description = calendarEvent.Description?.Trim() ?? string.Empty;
        await _store.AddAsync("calendarEvents", calendarEvent); //(Google Cloud, 2026).
        return CreatedAtAction(nameof(GetCalendar), new { id = calendarEvent.Id }, calendarEvent);
    }
}

/*
 Reference List

Microsoft Corporation, "Role-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles

Microsoft Corporation, "Controller Action Return Types in ASP.NET Core Web API," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types

Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */