using System.Security.Claims;
using Google.Cloud.Firestore;
using KoosMatliAdminApi.Localization;
using KoosMatliAdminApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliAdminApi.Controllers;

[Authorize] //(Microsoft Learn, 2026a).
[ApiController]
[Route("api/settings")]
public sealed class SettingsController(FirestoreDb db) : ControllerBase
{
    private DocumentReference Document =>
        db.Collection("userSettings").Document(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!); //(Microsoft Learn, 2026b).

    [HttpGet]
    public async Task<ActionResult<UserSettings>> Get()
    {
        var snapshot = await Document.GetSnapshotAsync();

        return Ok(snapshot.Exists
            ? snapshot.ConvertTo<UserSettings>()
            : new UserSettings());
    }

    [HttpPut]
    public async Task<ActionResult<UserSettings>> Put(
        UserSettings settings)
    {
        var language = Languages.Normalize(settings.Language);

        if (language is null)
            return BadRequest("InvalidLanguage");

        var theme = new[] { "System", "Light", "Dark" }
            .FirstOrDefault(x => x.Equals(
                settings.Theme,
                StringComparison.OrdinalIgnoreCase));

        if (theme is null)
            return BadRequest("InvalidTheme");

        await Document.SetAsync(
            new Dictionary<string, object>
            {
                ["Language"] = language,
                ["Theme"] = theme,
                ["NotificationsEnabled"] =
                    settings.NotificationsEnabled,
                ["ConfirmSensitiveActions"] =
                    settings.ConfirmSensitiveActions
            },
            SetOptions.MergeAll); //(Google Cloud, 2026).

        var snapshot = await Document.GetSnapshotAsync();

        return Ok(snapshot.ConvertTo<UserSettings>());
    }
}

/*Reference List
 Microsoft Corporation, "Create Web APIs with ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/
 Microsoft Corporation, "Claims-based Authorization in ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims
 Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
 */