using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net.Mail;

namespace KoosMatliAdminApi.Controllers;

[ApiController, Authorize, Route("api/admin/features")] //(Microsoft Learn, 2026a; Microsoft Learn, 2026b)
public sealed class AdminFeaturesController(FirebaseAuth auth, FirestoreDb db,FirestoreDataStore store) : ControllerBase
{
    public static readonly string[] PermissionCodes =
    [
        "dashboard.read", "users.read", "users.write",
        "applications.read", "applications.write", "staff.read",
        "school.read", "school.write", "documents.read",
        "documents.write", "backups.export", "roles.manage"
    ];

    private string Uid => User.FindFirstValue(ClaimTypes.NameIdentifier)!; //(Microsoft Learn, 2026a; Microsoft Learn, 2026b)

    private static bool Owner(UserRecord user) =>
        user.CustomClaims.TryGetValue("admin", out var admin) && //(Google Firebase, 2026a; Google Firebase, 2026b).
        admin is true &&
        !(user.CustomClaims.TryGetValue("accessManaged", out var managed) && managed is true);

    private static string Role(UserRecord user) =>
        Owner(user)
            ? "Admin"
            : user.CustomClaims.TryGetValue("role", out var role) //(Google Firebase, 2026a; Google Firebase, 2026b).
                ? role.ToString() ?? "Learner"
                : "Learner";

    private static string[] Permissions(UserRecord user) =>
        Owner(user)
            ? PermissionCodes
            : user.CustomClaims.TryGetValue("permissions", out var permissions)
                ? System.Text.Json.JsonSerializer.Deserialize<string[]>(
                    System.Text.Json.JsonSerializer.Serialize(permissions)) ?? []
                : [];

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var user = await auth.GetUserAsync(Uid); //(Google Firebase, 2026a; Google Firebase, 2026b).

        var settings = await db.Collection("userSettings")
            .Document(Uid).GetSnapshotAsync();

        return Ok(new
        {
            user.Uid,
            user.Email,
            DisplayName = !string.IsNullOrWhiteSpace(user.DisplayName)
                ? user.DisplayName
                : settings.Exists && settings.ContainsField("DisplayName")
                    ? settings.GetValue<string>("DisplayName")
                    : "",
            PhoneNumber = settings.Exists &&
                          settings.ContainsField("PhoneNumber")
                ? settings.GetValue<string>("PhoneNumber")
                : user.PhoneNumber ?? "",
            user.EmailVerified,
            user.Disabled,
            CreatedAt = user.UserMetaData.CreationTimestamp,
            LastSignInAt = user.UserMetaData.LastSignInTimestamp,
            Role = Owner(user) ? "Owner" : Role(user)
        });
    }

    public sealed record ProfileEdit( string DisplayName, string Email, string PhoneNumber);

    [HttpPut("profile")]
    public async Task<IActionResult> SaveProfile(ProfileEdit edit)
    {
        if (string.IsNullOrWhiteSpace(edit.DisplayName) ||
            edit.DisplayName.Length > 120 ||
            !MailAddress.TryCreate(edit.Email?.Trim(), out var address) ||
            address.Address != edit.Email?.Trim() ||
            (edit.PhoneNumber?.Length ?? 0) > 30)
        {
            return BadRequest("InvalidInput");
        }

        var user = await auth.GetUserAsync(Uid);

        bool changedEmail = !string.Equals(
            user.Email, edit.Email.Trim(),
            StringComparison.OrdinalIgnoreCase);

        if (changedEmail && (!long.TryParse(User.FindFirstValue("auth_time"), out var seconds) ||
             DateTimeOffset.UtcNow -
             DateTimeOffset.FromUnixTimeSeconds(seconds) >
             TimeSpan.FromMinutes(5)))
        {
            return BadRequest(new
            {
                message = "Sign in again, then change your email within five minutes."
            });
        }

        try
        {
            var args = new UserRecordArgs
            {
                Uid = Uid,
                DisplayName = edit.DisplayName.Trim()
            };

            if (changedEmail)
            {
                args.Email = edit.Email.Trim();
                args.EmailVerified = false;
            }

            await auth.UpdateUserAsync(args);
        }
        catch (FirebaseAuthException)
        {
            return BadRequest(new
            {
                message =   "Firebase could not update the account. Check that the email is valid and unused."
            });
        }

        await db.Collection("userSettings").Document(Uid).SetAsync(
            new Dictionary<string, object>
            {
                ["DisplayName"] = edit.DisplayName.Trim(),
                ["PhoneNumber"] = edit.PhoneNumber?.Trim() ?? ""
            }, SetOptions.MergeAll);

        var sharedProfile = db.Collection("users").Document(Uid);

        if ((await sharedProfile.GetSnapshotAsync()).Exists)
        {
            await sharedProfile.SetAsync(new Dictionary<string, object>
            {
                ["displayName"] = edit.DisplayName.Trim(),
                ["email"] = edit.Email.Trim(),
                ["phoneNumber"] = edit.PhoneNumber?.Trim() ?? ""
            }, SetOptions.MergeAll);
        }

        if (changedEmail)
            await auth.RevokeRefreshTokensAsync(Uid); //Google Firebase, 2026b

        return Ok(new { emailChanged = changedEmail });
    }

    [HttpPost("revoke-sessions")]
    public async Task<IActionResult> RevokeSessions()
    {
        await auth.RevokeRefreshTokensAsync(Uid);
        return NoContent();
    }

    [Authorize(Roles = "Admin"), HttpGet("access")]
    public async Task<IActionResult> Access()
    {
        var profiles = await store.GetAllAsync<User>("users"); //Google Firebase, 2026b

        var accounts = new List<UserRecord>();

        await foreach (var account in auth.ListUsersAsync(null)) //Google Firebase, 2026b
            accounts.Add(account);

        var rows = profiles.Select(profile =>
        {
            var account = accounts.FirstOrDefault(account =>
                string.Equals(
                    account.Email, profile.Email,
                    StringComparison.OrdinalIgnoreCase));

            return new
            {
                Uid = account?.Uid ?? "",
                ProfileId = profile.Id,
                Name = profile.FullName,
                profile.Email,
                Role = account is null ||
                       (!Owner(account) &&
                        !account.CustomClaims.ContainsKey("role"))
                    ? profile.Role
                    : Role(account),
                Permissions = account is null
                    ? Array.Empty<string>()
                    : Permissions(account),
                IsOwner = account is not null && Owner(account)
            };
        }).ToList();

        foreach (var account in accounts.Where(account =>
            !profiles.Any(profile => string.Equals(
                profile.Email, account.Email,
                StringComparison.OrdinalIgnoreCase))))
        {
            rows.Add(new
            {
                Uid = account.Uid,
                ProfileId = 0,
                Name = account.DisplayName ?? account.Email ?? account.Uid,
                Email = account.Email ?? "",
                Role = Role(account),
                Permissions = Permissions(account),
                IsOwner = Owner(account)
            });
        }

        return Ok(rows);
    }

    public sealed record AccessEdit(string Uid, int ProfileId, string Role, string[] Permissions);

    [Authorize(Roles = "Admin"), HttpPut("access")]
    public async Task<IActionResult> SaveAccess(AccessEdit edit)
    {
        if (!new[] { "Admin", "Teacher", "Parent", "Learner" }
                .Contains(edit.Role) ||
            edit.Permissions is null ||
            edit.Permissions.Any(permission =>
                !PermissionCodes.Contains(permission)))
        {
            return BadRequest("InvalidInput");
        }

        var permissions = edit.Role == "Admin"
            ? edit.Permissions.Concat(
                edit.Permissions
                    .Where(permission => permission.EndsWith(
                        ".write", StringComparison.Ordinal))
                    .Select(permission => permission[..^6] + ".read"))
                .Distinct().ToArray()
            : [];

        if (edit.Role == "Admin" &&
            !permissions.Contains("dashboard.read"))
        {
            return BadRequest(new
            {
                message = "An administrator needs Dashboard read permission."
            });
        }

        if (User.HasClaim("accessManaged", "true") &&
            permissions.Any(permission =>
                !User.HasClaim("permission", permission)))
        {
            return Forbid();
        }

        var profile = edit.ProfileId > 0
            ? await store.GetByIdAsync<User>("users", edit.ProfileId) //Google Firebase, 2026b
            : null;

        if (string.IsNullOrWhiteSpace(edit.Uid))
        {
            if (profile is null)
                return NotFound();

            if (edit.Role == "Admin" || edit.Permissions.Length > 0)
            {
                return BadRequest(new
                {
                    message = "Create a matching Firebase sign-in account before granting application access."
                });
            }
        }
        else
        {
            if (edit.Uid == Uid)
            {
                return BadRequest(new
                {
                    message = "You cannot change your own access."
                });
            }

            var target = await auth.GetUserAsync(edit.Uid); //Google Firebase, 2026b

            if (Owner(target))
            {
                return BadRequest(new
                {
                    message = "The bootstrap owner is protected. Change owner access only through trusted server setup."
                });
            }

            if (profile is not null &&
                !string.Equals(
                    profile.Email, target.Email,
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("InvalidInput");
            }

            var claims = target.CustomClaims.ToDictionary(
                claim => claim.Key, claim => claim.Value);

            claims["admin"] = edit.Role == "Admin";
            claims["role"] = edit.Role;
            claims["accessManaged"] = true;
            claims["permissions"] = permissions;

            await auth.SetCustomUserClaimsAsync(target.Uid, claims);
            await auth.RevokeRefreshTokensAsync(target.Uid);
        }

        if (profile is not null)
        {
            profile.Role = edit.Role;
            await store.SaveAsync("users", profile);
        }

        if (!string.IsNullOrWhiteSpace(edit.Uid))
        {
            var sharedProfile = db.Collection("users").Document(edit.Uid);

            if ((await sharedProfile.GetSnapshotAsync()).Exists)
            {
                await sharedProfile.SetAsync(
                    new Dictionary<string, object>
                    {
                        ["role"] = edit.Role.ToLowerInvariant()
                    }, SetOptions.MergeAll); //(Google Cloud, 2026).
            }
        }

        await db.Collection("adminAudit").AddAsync(
            new Dictionary<string, object>
            {
                ["ActorUid"] = Uid,
                ["TargetUid"] = edit.Uid,
                ["ProfileId"] = edit.ProfileId,
                ["Role"] = edit.Role,
                ["Permissions"] = permissions,
                ["At"] = Timestamp.GetCurrentTimestamp()
            });

        return NoContent();
    }

    [Authorize(Roles = "Admin"), HttpGet("backup")]
    public async Task<IActionResult> Backup() => Ok(new
    {
        FormatVersion = 1,
        ApplicationName = "KoosMatli",
        CreatedAt = DateTimeOffset.UtcNow,
        Users = await store.GetAllAsync<User>("users"),
        Applications = await store.GetAllAsync<AdmissionApplication>("applications"),
        Staff = await store.GetAllAsync<StaffMember>("staffMembers"),
        Classes = await store.GetAllAsync<SchoolClass>("schoolClasses"),
        Subjects = await store.GetAllAsync<Subject>("subjects"),
        SchoolDetails = await store.GetFirstAsync<SchoolDetails>("schoolDetails"),
        SchoolDates = await store.GetAllAsync<SchoolDate>("schoolDates"),
        CalendarEvents = await store.GetAllAsync<CalendarEvent>("calendarEvents"),
        Documents = await store.GetAllAsync<SchoolDocument>("schoolDocuments")
    });
}

/* Reference List
 
 Microsoft Corporation, "Role-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles
 Microsoft Corporation, "Claims-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims
 Google LLC, "Set and Validate Custom Claims via Admin SDK," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/custom-claims
 Google LLC, "Manage Users in Firebase with the Admin SDK," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/manage-users
 Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */