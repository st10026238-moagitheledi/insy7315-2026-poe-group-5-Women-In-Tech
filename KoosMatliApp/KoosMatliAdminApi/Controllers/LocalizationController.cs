using System.Globalization;
using KoosMatliAdminApi.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace KoosMatliAdminApi.Controllers;
[AllowAnonymous]
[ApiController]
[Route("api/localization")] //(Microsoft Learn, 2026a).
public sealed class LocalizationController(IStringLocalizer<Messages> messages) : ControllerBase //(Microsoft Learn, 2026b).
{
    [HttpGet("languages")]
    public IActionResult LanguagesList() => Ok(new[] {
        new { code = "en-ZA", name = "English" }, new { code = "tn-ZA", name = "Setswana" },
        new { code = "nso-ZA", name = "Sepedi" }
    });
    [HttpGet("resources")]
    public IActionResult Resources() => Ok(new {
        culture = CultureInfo.CurrentUICulture.Name, //(Microsoft Learn, 2026c).
        strings = messages.GetAllStrings(true).ToDictionary(x => x.Name, x => x.Value)
    });
}
/*
 Reference List

Microsoft Corporation, "Create Web APIs with ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/
Microsoft Corporation, "Globalize and Localize ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization
Microsoft Corporation, "CultureInfo Class (System.Globalization)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo
 */