using System.Collections;
using System.Globalization;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
namespace KoosMatliApp.Localization;
public static class L
{
    public static readonly string[] Codes = ["en-ZA", "tn-ZA", "nso-ZA"];
    public static CultureInfo Culture { get; private set; } = CultureInfo.GetCultureInfo("en-ZA"); //(Microsoft Learn, 2026a).
    private static readonly ResourceManager Manager = new("KoosMatliApp.Resources.Strings.AppStrings", typeof(L).Assembly); //(Microsoft Learn, 2026b).
    private static string Key(string value) => "T_" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)))[..12];
    public static string T(string english) => string.IsNullOrEmpty(english) ? english : Manager.GetString(Key(english), Culture) ?? english;
    public static string F(string english, params object[] args) => string.Format(Culture, T(english), args);
    public static void SetCulture(string code, bool persist = true) {
        Culture = CultureInfo.GetCultureInfo(Codes.Contains(code) ? code : "en-ZA");
        CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = Culture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = Culture;
        if (persist) Preferences.Default.Set("KoosMatli.Language", Culture.Name);
        if (Application.Current is not { } app) return; //(Microsoft Learn, 2026c).
        var neutral = Manager.GetResourceSet(CultureInfo.InvariantCulture, true, true)!;
        foreach (DictionaryEntry item in neutral) {
            var key = (string)item.Key;
            app.Resources[key] = Manager.GetString(key, Culture) ?? (string)item.Value!;
        }
    }
}

/*Reference List
 
Microsoft Corporation, "Globalize and Localize .NET Applications," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/core/extensions/localization

Microsoft Corporation, "ResourceManager Class (System.Resources)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.resources.resourcemanager

Microsoft Corporation, "Cryptographic Hash Classes (System.Security.Cryptography)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography

Microsoft Corporation, "Preferences and Resource Dictionaries in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-resources
 */
