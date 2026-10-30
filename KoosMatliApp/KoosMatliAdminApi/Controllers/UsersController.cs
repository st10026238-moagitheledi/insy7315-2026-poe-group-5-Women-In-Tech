using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/users")]
public class UsersController : ControllerBase
{
    private readonly FirestoreDataStore _store; //(Microsoft Learn, 2026a).

    public UsersController(FirestoreDataStore store) => _store = store;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers() => //(Microsoft Learn, 2026b).
        Ok(await _store.GetSchoolUsersAsync());

    [HttpGet("role/{role}")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(
        string role)
    {
        var users = await _store.GetSchoolUsersAsync();

        return Ok(users.Where(user => string.Equals(
            user.Role, role, StringComparison.OrdinalIgnoreCase)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _store.GetByIdAsync<User>("users", id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName))
            return BadRequest("First name is required."); //(Microsoft Learn, 2026b).

        if (string.IsNullOrWhiteSpace(user.LastName))
            return BadRequest("Surname is required.");

        if (string.IsNullOrWhiteSpace(user.Email))
            return BadRequest("Email is required.");

        if (string.IsNullOrWhiteSpace(user.Role))
            return BadRequest("A role is required.");

        var matchingRole = new[] { "Teacher", "Parent", "Learner" }
            .FirstOrDefault(role => role.Equals(
                user.Role.Trim(), StringComparison.OrdinalIgnoreCase));

        if (matchingRole is null)
            return BadRequest("Role must be Teacher, Parent or Learner.");

        var email = user.Email.Trim();

        if (!System.Net.Mail.MailAddress.TryCreate(email, out var parsed) || //(Microsoft Learn, 2026c).
            parsed.Address != email)
        {
            return BadRequest("InvalidInput");
        }

        var users = await _store.GetSchoolUsersAsync();

        if (users.Any(existing => string.Equals(
            existing.Email, email, StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict("A user with this email already exists.");
        }

        user.Id = 0;
        user.FirstName = user.FirstName.Trim();
        user.LastName = user.LastName.Trim();
        user.Email = email;
        user.PhoneNumber = user.PhoneNumber?.Trim() ?? "";
        user.Role = matchingRole;

        try
        {
            await _store.AddAsync("users", user, email);
        }
        catch (DuplicateProfileException) //(Google Cloud, 2026).
        {
            return Conflict("A user with this email already exists.");
        }

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}

/*
 Reference List

Microsoft Corporation, "Role-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles
Microsoft Corporation, "Controller Action Return Types in ASP.NET Core Web API," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types
Microsoft Corporation, "MailAddress Class (System.Net.Mail)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress
Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */