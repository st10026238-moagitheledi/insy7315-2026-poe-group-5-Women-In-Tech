using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] //(Microsoft Learn, 2026a).
[ApiController]
[Route("api/admin/staff")]
public class StaffController : ControllerBase
{
    private readonly FirestoreDataStore _store;
    public StaffController(FirestoreDataStore store) => _store = store;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffMember>>> GetAll() => //(Microsoft Learn, 2026b).
        Ok(await _store.GetAllAsync<StaffMember>("staffMembers"));

    [HttpGet("classes")]
    public async Task<ActionResult<IEnumerable<SchoolClass>>> Classes() => Ok(await _store.GetAllAsync<SchoolClass>("schoolClasses"));

    [HttpGet("subjects")]
    public async Task<ActionResult<IEnumerable<Subject>>> Subjects() => Ok(await _store.GetAllAsync<Subject>("subjects")); //(Microsoft Learn, 2026b).

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StaffMember>> Get(int id) //(Microsoft Learn, 2026b).
    {
        var member = await _store.GetByIdAsync<StaffMember>("staffMembers", id); //(Google Cloud, 2026).
        return member is null ? NotFound() : Ok(member);
    }
}

/*Reference List
 
Microsoft Corporation, "Role-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles

Microsoft Corporation, "Controller Action Return Types in ASP.NET Core Web API," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types

Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */
