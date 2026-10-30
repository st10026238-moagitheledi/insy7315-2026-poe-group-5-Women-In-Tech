namespace KoosMatliAdminApi.Localization;
public class Messages { }
public static class Languages
{
    public static readonly string[] Codes = ["en-ZA", "tn-ZA", "nso-ZA"];
    public static string? Normalize(string? code) => code?.Trim().ToLowerInvariant() switch {
        "en" or "en-za" => "en-ZA", "tn" or "tn-za" => "tn-ZA", "nso" or "nso-za" => "nso-ZA", _ => null
    };
}
