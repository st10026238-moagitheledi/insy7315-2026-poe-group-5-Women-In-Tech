using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Security.Claims;

namespace KoosMatliAdminApi.Controllers;

[ApiController]
public sealed class AccountSecurityController(
    FirebaseAuth auth,
    IConfiguration configuration) : ControllerBase
{
    // Confirms API availability
    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Status() => //(Microsoft Learn, 2026a).
        Ok(new
        {
            status = "API running",
            languages = "/api/localization/languages"
        });

    [Authorize]
    [HttpPost("api/account/password-reset")]
    public async Task<IActionResult> ResetPassword(CancellationToken cancellationToken)
    {
        var key = configuration["Firebase:WebApiKey"];

        if (string.IsNullOrWhiteSpace(key))
        {
            return StatusCode(503, new
            {
                message = "Configure Firebase:WebApiKey in the backend before sending password reset emails."
            });
        }

        // Only reset the authenticated user's password.
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier); //(Microsoft Learn, 2026b).

        if (string.IsNullOrWhiteSpace(uid))
            return Unauthorized();

        var user = await auth.GetUserAsync(uid, cancellationToken); //(Google Firebase, 2026a).

        if (string.IsNullOrWhiteSpace(user.Email) || !user.ProviderData.Any(p => p.ProviderId == "password"))
        {
            return BadRequest(new
            {
                message = "Password reset is available for email/password accounts."
            });
        }

        using var client = new HttpClient //(Google Firebase, 2026b).
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        using var response = await client.PostAsJsonAsync("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" +
            Uri.EscapeDataString(key),
            new
            {
                requestType = "PASSWORD_RESET",
                email = user.Email
            },
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(502, new
            {
                message = "Firebase could not send the reset email. Check the Web API key and email/password provider, or try again later."
            });
        }

        return NoContent();
    }
}

/*
 Reference List

Microsoft Corporation, "Create Web APIs with ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/

Microsoft Corporation, "Overview of ASP.NET Core Security," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/

Google LLC, "Manage Users in Firebase," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/manage-users

Google LLC, "Google Identity Toolkit API Reference (accounts:sendOobCode)," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/identity-toolkit/docs/reference/rest/v1/accounts/sendOobCode
 */