using KoosMatliAdminApi.DTOs;
using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin/dashboard")]
public class AdminDashboardController : ControllerBase
{
    private readonly FirestoreDataStore _store;

    public AdminDashboardController(FirestoreDataStore store) =>
        _store = store;

    [HttpGet]
    public async Task<ActionResult<AdminDashboardDto>> GetDashboard()
    {
        var users = await _store.GetSchoolUsersAsync();
        var staff = await _store.GetAllAsync<StaffMember>("staffMembers");

        var applications =
            await _store.GetAllAsync<AdmissionApplication>("applications");

        return Ok(new AdminDashboardDto
        {
            TotalLearners = users.Count(user =>
                user.Role.Equals("Learner", StringComparison.OrdinalIgnoreCase)),

            TotalTeachers = CountTeachers(users, staff),

            TotalParents = users.Count(user =>
                user.Role.Equals("Parent", StringComparison.OrdinalIgnoreCase)),

            NewApplications = applications.Count(application =>
                application.Status.Equals(
                    "New", StringComparison.OrdinalIgnoreCase)),

            PendingApplications = applications.Count(application =>
                application.Status.Equals(
                    "Pending", StringComparison.OrdinalIgnoreCase)),

            AnnouncementsSent = 0
        });
    }

    public static int CountTeachers(
        IEnumerable<User> users,
        IEnumerable<StaffMember> staff) =>
        users
            .Where(user => user.Role.Equals(
                "Teacher", StringComparison.OrdinalIgnoreCase))
            .Select(user => string.IsNullOrWhiteSpace(user.Email)
                ? $"user:{user.Id}"
                : user.Email.Trim().ToUpperInvariant())
            .Concat(staff
                .Where(member => member.Position.Equals(
                    "Teacher", StringComparison.OrdinalIgnoreCase))
                .Select(member => string.IsNullOrWhiteSpace(member.Email)
                    ? $"staff:{member.Id}"
                    : member.Email.Trim().ToUpperInvariant()))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
}