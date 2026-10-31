using System.Text.Json;
namespace KoosMatliApp.Services;
public static class SettingsAuditStore
{
    private static string Key(string uid) => "KoosMatli.Audit." + uid;
    public sealed record Entry(DateTimeOffset Time, string Action);
    public static List<Entry> Read(string uid) {
        try { return JsonSerializer.Deserialize<List<Entry>>(Preferences.Default.Get(Key(uid), "[]")) ?? []; }
        catch (JsonException) { return []; }
    }
    public static void Log(string uid, string action) {
        var entries = Read(uid); entries.Insert(0, new(DateTimeOffset.UtcNow, action));
        Preferences.Default.Set(Key(uid), JsonSerializer.Serialize(entries.Take(200)));
    }
    public static void Clear(string uid) => Preferences.Default.Remove(Key(uid));
}
