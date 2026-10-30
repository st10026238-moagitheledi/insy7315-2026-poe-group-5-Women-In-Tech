using KoosMatliAdminApi.DTOs;
using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] //(Microsoft Learn, 2026a; Microsoft Learn, 2026b)
[ApiController]
[Route("api/admin/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly FirestoreDataStore _store; //(Google Cloud, 2026).
    public ApplicationsController(FirestoreDataStore store) => _store = store;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdmissionApplication>>> GetAll() =>
        Ok(await _store.GetAllAsync<AdmissionApplication>("applications")); //(Google Firebase, 2026a; Google Firebase, 2026b).

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdmissionApplication>> Get(int id)
    {
        var application = await _store.GetByIdAsync<AdmissionApplication>("applications", id); //(Google Firebase, 2026a; Google Firebase, 2026b).
        return application is null ? NotFound() : Ok(application);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateApplicationStatusDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Status)) return BadRequest("A status is required.");
        var status = new[] { "New", "Pending", "Approved", "Rejected" }
            .FirstOrDefault(x => x.Equals(dto.Status.Trim(), StringComparison.OrdinalIgnoreCase));
        if (status is null) return BadRequest("InvalidStatus");
        return await _store.UpdateStatusAsync(id, status) ? NoContent() : NotFound();
    }
}

/*
 Reference List

Microsoft Corporation, "Role-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles

Microsoft Corporation, "Claims-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims

Google LLC, "Set and Validate Custom Claims via Admin SDK," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/custom-claims

Google LLC, "Manage Users in Firebase with the Admin SDK," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/manage-users

Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */
