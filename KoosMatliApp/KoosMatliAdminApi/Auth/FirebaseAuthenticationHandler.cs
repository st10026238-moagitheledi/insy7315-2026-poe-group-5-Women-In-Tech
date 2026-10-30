using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using KoosMatliAdminApi.Localization;

namespace KoosMatliAdminApi.Auth;
public sealed class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseAuth _auth;
    private readonly IStringLocalizer<Messages> _messages;
    public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, FirebaseAuth auth, IStringLocalizer<Messages> messages)
        : base(options, logger, encoder) { _auth = auth; _messages = messages; } //(Google Firebase, 2026a).
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var header = Request.Headers.Authorization.ToString(); //(Microsoft Learn, 2026a; Microsoft Learn, 2026b)
        if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return AuthenticateResult.NoResult();
        var token = header[7..].Trim();
        if (token.Length == 0) return AuthenticateResult.Fail("Missing token.");
        try {
            var decoded = await _auth.VerifyIdTokenAsync(token, true);
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, decoded.Uid) };
            // Roles come only from server-issued custom claims, never from editable profile documents.
            if ((decoded.Claims.TryGetValue("admin", out var admin) && admin is true) ||
                (decoded.Claims.TryGetValue("role", out var role) && role is string r && r == "Admin"))
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal( //(Microsoft Learn, 2026a; Microsoft Learn, 2026b).
                new ClaimsIdentity(claims, Scheme.Name)), Scheme.Name));
        } catch (FirebaseAuthException) { return AuthenticateResult.Fail("Invalid or revoked token."); } //(Google Firebase, 2026a).
    }
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties) {
        Response.StatusCode = 401; Response.Headers.WWWAuthenticate = "Bearer";
        await Response.WriteAsJsonAsync(new { message = _messages["Unauthorized"].Value });
    }
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties) {
        Response.StatusCode = 403;
        await Response.WriteAsJsonAsync(new { message = _messages["Forbidden"].Value });
    }
}
/*Reference List
 
 Microsoft Corporation, "AuthenticationHandler<TOptions> Class," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1

 Microsoft Corporation, "Overview of ASP.NET Core Authentication," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/

 Google LLC, "Verify ID Tokens," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/verify-id-tokens

 Google LLC, "Control Access with Custom Claims and Security Rules," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/auth/admin/custom-claims

 Microsoft Corporation, "Globalize and Localize ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization
 */