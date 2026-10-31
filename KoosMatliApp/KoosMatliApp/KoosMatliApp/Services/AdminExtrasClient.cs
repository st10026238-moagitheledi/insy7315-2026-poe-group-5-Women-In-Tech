using System.Net.Http.Json;
using KoosMatliApp.Models;

namespace KoosMatliApp.Services;

public sealed class AdminExtrasClient(HttpClient http)
{
    public static readonly string[] PermissionCodes =
    [
        "dashboard.read",
        "users.read", "users.write",
        "applications.read", "applications.write",
        "staff.read",
        "school.read", "school.write",
        "documents.read", "documents.write",
        "backups.export",
        "roles.manage"
    ];

    public sealed class Account
    {
        public string Uid { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool EmailVerified { get; set; }
        public bool Disabled { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastSignInAt { get; set; }
        public string Role { get; set; } = "";
    }

    public sealed class AccessUser
    {
        public string Uid { get; set; } = "";
        public int ProfileId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "Learner";
        public string[] Permissions { get; set; } = [];
        public bool IsOwner { get; set; }

        public override string ToString() =>
            string.IsNullOrWhiteSpace(Name)
                ? Email
                : $"{Name} ({Email})";
    }

    private sealed class EmailResult
    {
        public bool EmailChanged { get; set; }
    }

    public Task<Account> GetAccountAsync() =>
        GetAsync<Account>("api/admin/features/profile");

    public Task<List<AccessUser>> GetAccessAsync() =>
        GetAsync<List<AccessUser>>("api/admin/features/access");

    private async Task<T> GetAsync<T>(string path)
    {
        using var response = await http.GetAsync(path);

        await AdminApiService.EnsureSuccessAsync(response);

        return await response.Content.ReadFromJsonAsync<T>()
            ?? throw new HttpRequestException(
                L.T("The server returned no saved record."));
    }

    public async Task<bool> SaveAccountAsync(
        string name,
        string email,
        string phone)
    {
        using var response = await http.PutAsJsonAsync(
            "api/admin/features/profile",
            new
            {
                displayName = name,
                email,
                phoneNumber = phone
            });

        await AdminApiService.EnsureSuccessAsync(response);

        var result = await response.Content
            .ReadFromJsonAsync<EmailResult>();

        return result?.EmailChanged == true;
    }

    public async Task SaveAccessAsync(
        AccessUser user,
        string role,
        string[] permissions)
    {
        using var response = await http.PutAsJsonAsync(
            "api/admin/features/access",
            new { user.Uid, user.ProfileId, role, permissions });

        await AdminApiService.EnsureSuccessAsync(response);
    }

    public async Task RevokeSessionsAsync()
    {
        using var response = await http.PostAsync(
            "api/admin/features/revoke-sessions", null);

        await AdminApiService.EnsureSuccessAsync(response);
    }

    public async Task<string> ExportAsync()
    {
        using var response = await http.GetAsync(
            "api/admin/features/backup");

        await AdminApiService.EnsureSuccessAsync(response);

        var path = Path.Combine(
            FileSystem.CacheDirectory,
            $"KoosMatliBackup_{DateTime.UtcNow:yyyyMMdd_HHmmss}.json");

        await File.WriteAllTextAsync(
            path, await response.Content.ReadAsStringAsync());

        return path;
    }

    public async Task UploadAsync(FileResult file)
    {
        await using var input = await file.OpenReadAsync();

        if (input.CanSeek && input.Length > 10 * 1024 * 1024)
        {
            throw new InvalidOperationException(
                "The file must be 10 MB or smaller.");
        }

        using var form = new MultipartFormDataContent();

        form.Add(new StreamContent(input), "file", file.FileName);

        form.Add(
            new StringContent(
                Path.GetFileNameWithoutExtension(file.FileName)),
            "title");

        using var response = await http.PostAsync(
            "api/admin/documents", form);

        await AdminApiService.EnsureSuccessAsync(response);
    }

    public async Task OpenAsync(SchoolDocument document)
    {
        using var response = await http.GetAsync(
            $"api/admin/documents/{document.Id}/file");

        await AdminApiService.EnsureSuccessAsync(response);

        var path = Path.Combine(
            FileSystem.CacheDirectory,
            Guid.NewGuid().ToString("N") + "_" +
            Path.GetFileName(document.FileName));

        await using (var output = File.Create(path))
            await response.Content.CopyToAsync(output);

        await Launcher.Default.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(path)
        });
    }
}

public static class AdminText
{
    private static readonly Dictionary<string, (string Tn, string Nso)>
        Words = new()
        {
            ["Select a user"] =
                ("Tlhopha modirisi", "Kgetha modiriši"),

            ["Enter a display name and email address."] =
                ("Tsenya leina le aterese ya imeile.",
                 "Tsenya leina le aterese ya imeile."),

            ["Upload Document"] =
                ("Tsenya tokomane", "Tsenya tokomane"),

            ["Last sign-in"] =
                ("Go tsena ga bofelo", "Go tsena ga mafelelo"),

            ["Account created"] =
                ("Akhaonto e dirilwe", "Akhaonto e hlotšwe"),

            ["Email verified"] =
                ("Imeile e netefaditswe", "Imeile e netefaditšwe"),

            ["Yes"] = ("Ee", "Ee"),
            ["No"] = ("Nnyaa", "Aowa"),
            ["Owner"] = ("Mong", "Mong"),
            ["Read"] = ("Bala", "Bala"),
            ["Write"] = ("Fetola", "Fetoša"),
            ["Manage"] = ("Laola", "Laola"),

            ["End all sign-in sessions"] =
                ("Fedisa go tsena mo didirisweng tsotlhe",
                 "Fediša go tsena didirišweng ka moka"),

            ["End all sign-in sessions for this account?"] =
                ("Fedisa go tsena ga akhaonto eno mo didirisweng tsotlhe?",
                 "Fediša go tsena ga akhaonto ye didirišweng ka moka?"),

            ["Email changes require a recent sign-in and will end your session."] =
                ("Go fetola imeile go tlhoka go tsena sesha mme go tla fedisa nako ya gago ya go tsena.",
                 "Go fetoša imeile go nyaka go tsena gape gomme go tla fediša nako ya gago ya go tsena."),

            ["Sign in again with your new email."] =
                ("Tsena gape ka imeile ya gago e ntšha.",
                 "Tsena gape ka imeile ya gago ye mpsha."),

            ["You cannot change owner access or your own access here."] =
                ("Ga o ka ke wa fetola tetla ya mong kgotsa ya gago fano.",
                 "O ka se fetoše tumelelo ya mong goba ya gago mo."),

            ["No sign-in account is linked. Only the school role can be changed."] =
                ("Ga go na akhaonto ya go tsena e e golagantsweng. Go ka fetolwa fela seabe sa sekolo.",
                 "Ga go na akhaonto ya go tsena ye e kgokagantšwego. Go ka fetošwa fela tema ya sekolo."),

            ["Module permissions apply to administrators. Other roles use their own application areas."] =
                ("Ditetla tsa dikarolo di ama batsamaisi. Diabe tse dingwe di dirisa dikarolo tsa tsone.",
                 "Ditumelelo tša dikarolo di ama balaodi. Ditema tše dingwe di šomiša dikarolo tša tšona."),

            ["This exports school records only. Uploaded files and sign-in accounts are not included."] =
                ("Seno se romela direkoto tsa sekolo fela. Difaele le diakhaonto tsa go tsena ga di akarediwe.",
                 "Se se romela direkhoto tša sekolo fela. Difaele le diakhaonto tša go tsena ga di akaretšwe."),

            ["The server took too long. Check that the API is running, then try again."] =
                ("Seva e tsere nako e telele. Lekola gore API e a dira, mme o leke gape.",
                 "Seva e tšere nako ye telele. Lekola gore API e a šoma, gomme o leke gape.")
        };

    public static string T(string text) =>
        Words.TryGetValue(text, out var value)
            ? L.Culture.Name switch
            {
                "tn-ZA" => value.Tn,
                "nso-ZA" => value.Nso,
                _ => text
            }
            : L.T(text);

    public static string Permission(string code)
    {
        var parts = code.Split('.');

        var module = parts[0] switch
        {
            "dashboard" => "Dashboard",
            "users" => "Users",
            "applications" => "Applications",
            "staff" => "Staff",
            "school" => "School",
            "documents" => "Documents",
            "backups" => "Backups",
            "roles" => "Roles & Permissions",
            _ => parts[0]
        };

        var operation = parts[1] switch
        {
            "read" => T("Read"),
            "write" => T("Write"),
            "manage" => T("Manage"),
            "export" => L.T("Export JSON backup"),
            _ => parts[1]
        };

        return L.T(module) + " — " + operation;
    }
}