using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Admin;

public abstract class SettingsPage : ContentPage
{
    protected readonly SettingsService Settings =
        App.Services!.GetRequiredService<SettingsService>();

    protected readonly AdminSession Auth =
        App.Services!.GetRequiredService<AdminSession>();

    protected readonly AdminExtrasClient Client =
        new(App.Services!.GetRequiredService<HttpClient>());

    protected readonly VerticalStackLayout Body = new()
    {
        Padding = 24,
        Spacing = 14,
        MaximumWidthRequest = 720
    };

    protected readonly Label Status = new()
    {
        TextColor = Colors.Red
    };

    private readonly List<Button> _actions = [];
    private bool _busy;

    protected SettingsPage(string title)
    {
        Title = L.T(title);

        var back = new Button
        {
            Text = L.T("← Back"),
            HorizontalOptions = LayoutOptions.Start
        };

        back.Clicked += async (_, _) =>
            await Shell.Current.GoToAsync("//profile");

        Body.Add(back);
        Body.Add(Status);

        Content = new ScrollView { Content = Body };
    }

    protected static string Words(string en, string tn, string nso) =>
        L.Culture.Name switch
        {
            "tn-ZA" => tn,
            "nso-ZA" => nso,
            _ => en
        };

    protected void Text(string text) =>
        Body.Add(new Label { Text = AdminText.T(text) });

    protected Button Action(string title, Func<Task> action)
    {
        var button = new Button { Text = AdminText.T(title) };

        button.Clicked += async (_, _) => await RunAsync(action);

        _actions.Add(button);
        Body.Add(button);
        return button;
    }

    protected async Task RunAsync(Func<Task> action)
    {
        if (_busy)
            return;

        if (!Auth.IsConnected)
        {
            Status.Text =
                L.T("Connect the signed-in user before loading admin data.");
            return;
        }

        _busy = true;
        Status.Text = "";

        foreach (var button in _actions)
            button.IsEnabled = false;

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            _busy = false;

            foreach (var button in _actions)
                button.IsEnabled = true;
        }
    }

    protected Task Saved() =>
        DisplayAlertAsync(
            L.T("Saved"),
            L.T("Your settings have been saved."),
            L.T("OK"));

    protected static void LeaveSession()
    {
        App.ClearAdminSession();

#if DEBUG
        if (Application.Current?.Windows.FirstOrDefault() is { } window)
            window.Page = new KoosMatliApp.Views.AdminTestSignInPage();
#else
        App.ShowShell();
#endif
    }
}

public sealed class AccountSettingsPage : SettingsPage
{
    private readonly Entry _name = new()
    {
        MaxLength = 120,
        Placeholder = L.T("Display name")
    };

    private readonly Entry _email = new()
    {
        Keyboard = Keyboard.Email
    };

    private readonly Entry _phone = new()
    {
        Keyboard = Keyboard.Telephone,
        MaxLength = 30
    };

    private bool _loaded;

    public AccountSettingsPage() : base("Account Settings")
    {
        Text("Display name");
        Body.Add(_name);

        Text("Email address");
        Body.Add(_email);

        Text("Phone number");
        Body.Add(_phone);

        Text("Email changes require a recent sign-in and will end your session.");

        Action("Save", async () =>
        {
            if (string.IsNullOrWhiteSpace(_name.Text) ||
                string.IsNullOrWhiteSpace(_email.Text))
            {
                Status.Text =
                    AdminText.T("Enter a display name and email address.");
                return;
            }

            bool emailChanged = await Client.SaveAccountAsync(
                _name.Text.Trim(),
                _email.Text.Trim(),
                _phone.Text?.Trim() ?? "");

            if (emailChanged)
            {
                await DisplayAlertAsync(
                    L.T("Saved"),
                    AdminText.T("Sign in again with your new email."),
                    L.T("OK"));

                LeaveSession();
                return;
            }

            await Settings.LoadAsync();
            await Saved();
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_loaded)
            return;

        await RunAsync(async () =>
        {
            var account = await Client.GetAccountAsync();

            _name.Text = account.DisplayName;
            _email.Text = account.Email;
            _phone.Text = account.PhoneNumber;

            _loaded = true;
        });
    }
}

public sealed class SystemSettingsPage : SettingsPage
{
    public SystemSettingsPage() : base("System Settings")
    {
        Text("Language");

        var language = new Picker
        {
            ItemsSource = new[] { "English", "Setswana", "Sepedi" },
            SelectedIndex = Array.IndexOf(
                L.Codes, Settings.Current.Language)
        };

        Body.Add(language);

        Text("Theme");

        var themes = new[] { "System", "Light", "Dark" };

        var theme = new Picker
        {
            ItemsSource = themes.Select(L.T).ToList(),
            SelectedIndex = Array.IndexOf(
                themes, Settings.Current.Theme)
        };

        Body.Add(theme);

        Text("Notifications");

        var notifications = new Switch
        {
            IsToggled = Settings.Current.NotificationsEnabled
        };

        Body.Add(notifications);

        Text("This saves your notification preference. Push delivery requires a notification service.");

        Action("Save", async () =>
        {
            if (language.SelectedIndex < 0 || theme.SelectedIndex < 0)
            {
                Status.Text = L.T("Choose a language and theme.");
                return;
            }

            var next = Settings.Copy();

            next.Language = L.Codes[language.SelectedIndex];
            next.Theme = themes[theme.SelectedIndex];
            next.NotificationsEnabled = notifications.IsToggled;

            await Settings.SaveAsync(next);

            App.ShowShell();
            await Shell.Current.GoToAsync("//profile");

            await Shell.Current.DisplayAlertAsync(
                L.T("Saved"),
                L.T("Your settings have been saved."),
                L.T("OK"));
        });
    }
}

public sealed class SecuritySettingsPage : SettingsPage
{
    private readonly Label _details = new();

    public SecuritySettingsPage() : base("Security")
    {
        Body.Add(_details);

        Body.Add(new Label
        {
            Text = Words(
                "Ask before exporting school records",
                "Botsa pele o romela direkoto tsa sekolo",
                "Botšiša pele o romela direkhoto tša sekolo")
        });

        var confirm = new Switch
        {
            IsToggled = Settings.Current.ConfirmSensitiveActions
        };

        Body.Add(confirm);

        Action("Save", async () =>
        {
            var next = Settings.Copy();
            next.ConfirmSensitiveActions = confirm.IsToggled;

            await Settings.SaveAsync(next);
            await Saved();
        });

        var reset = Action("Reset password", async () =>
        {
            bool proceed = await DisplayAlertAsync(
                Title,
                Words(
                    "Send a password reset email to your account?",
                    "Romela imeile ya go fetola phasewete?",
                    "Romela imeile ya go fetoša phasewete?"),
                L.T("Continue"),
                L.T("Cancel"));

            if (!proceed)
                return;

            using var response =
                await App.Services!.GetRequiredService<HttpClient>()
                    .PostAsync("api/account/password-reset", null);

            await AdminApiService.EnsureSuccessAsync(response);

            await DisplayAlertAsync(
                Title,
                Words(
                    "Password reset email sent. Check your inbox and spam folder.",
                    "Imeile e rometswe. Lekola imeile le foldara ya spam.",
                    "Imeile e rometšwe. Lekola imeile le foltara ya spam."),
                L.T("OK"));
        });

        reset.Text = Words(
            "Reset password",
            "Fetola phasewete",
            "Fetoša phasewete");

        Action("End all sign-in sessions", async () =>
        {
            bool proceed = await DisplayAlertAsync(
                L.T("Confirm"),
                AdminText.T("End all sign-in sessions for this account?"),
                L.T("Continue"),
                L.T("Cancel"));

            if (!proceed)
                return;

            await Client.RevokeSessionsAsync();
            LeaveSession();
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RunAsync(async () =>
        {
            var account = await Client.GetAccountAsync();

            string Date(DateTime? value) =>
                value?.ToLocalTime().ToString("g", L.Culture) ?? "—";

            _details.Text =
                $"{AdminText.T("Last sign-in")}: {Date(account.LastSignInAt)}\n" +
                $"{AdminText.T("Account created")}: {Date(account.CreatedAt)}\n" +
                $"{AdminText.T("Email verified")}: " +
                $"{AdminText.T(account.EmailVerified ? "Yes" : "No")}\n" +
                $"{L.T("Role")}: {AdminText.T(account.Role)}";
        });
    }
}

public sealed class RolesPermissionsPage : SettingsPage
{
    private readonly Picker _users = new()
    {
        Title = AdminText.T("Select a user")
    };

    private readonly Picker _role = new();

    private readonly Dictionary<string, CheckBox> _permissions = [];

    private static readonly
        (string Label, string Tn, string Nso, string[] Codes)[] Groups =
    [
        (
            "Manage users",
            "Laola badirisi",
            "Laola badiriši",
            ["users.read", "users.write", "staff.read"]
        ),
        (
            "Manage applications",
            "Laola dikopo",
            "Laola dikgopelo",
            ["applications.read", "applications.write"]
        ),
        (
            "Manage school information",
            "Laola tshedimosetso ya sekolo",
            "Laola tshedimošo ya sekolo",
            ["school.read", "school.write"]
        ),
        (
            "Manage documents",
            "Laola ditokomane",
            "Laola ditokomane",
            ["documents.read", "documents.write"]
        ),
        (
            "Manage access and backups",
            "Laola ditetla le dikhopi",
            "Laola ditumelelo le dikopi",
            ["roles.manage", "backups.export"]
        )
    ];

    private sealed record Choice(string Value, string Text);

    public RolesPermissionsPage() : base("Roles & Permissions")
    {
        Text("Users");
        Body.Add(_users);

        Text("Role");
        Body.Add(_role);

        _role.ItemDisplayBinding = new Binding(nameof(Choice.Text));

        foreach (var group in Groups)
        {
            var check = new CheckBox();
            _permissions[group.Label] = check;

            Body.Add(new HorizontalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    check,
                    new Label
                    {
                        Text = Words(group.Label, group.Tn, group.Nso),
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            });
        }

        _users.SelectedIndexChanged += (_, _) => SelectUser();
        _role.SelectedIndexChanged += (_, _) => SetEnabled();

        Action("Save", async () =>
        {
            if (_users.SelectedItem is not AdminExtrasClient.AccessUser user ||
                _role.SelectedItem is not Choice choice)
                return;

            if (user.IsOwner || user.Uid == Auth.Uid)
            {
                Status.Text = AdminText.T(
                    "You cannot change owner access or your own access here.");
                return;
            }

            var permissions = choice.Value == "Admin"
                ? Groups
                    .Where(g => _permissions[g.Label].IsChecked)
                    .SelectMany(g => g.Codes)
                    .Append("dashboard.read")
                    .Distinct()
                    .ToArray()
                : [];

            await Client.SaveAccessAsync(user, choice.Value, permissions);
            await LoadAsync();
            await Saved();
        });

        Action("Refresh", LoadAsync);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunAsync(LoadAsync);
    }

    private async Task LoadAsync()
    {
        _users.ItemsSource = await Client.GetAccessAsync();
        _users.SelectedIndex = -1;
        _role.ItemsSource = null;

        foreach (var check in _permissions.Values)
            check.IsEnabled = false;
    }

    private void SelectUser()
    {
        if (_users.SelectedItem is not AdminExtrasClient.AccessUser user)
            return;

        var codes = string.IsNullOrEmpty(user.Uid)
            ? new[] { "Teacher", "Parent", "Learner" }
            : new[] { "Admin", "Teacher", "Parent", "Learner" };

        var choices = codes
            .Select(code => new Choice(code, L.T(code)))
            .ToList();

        _role.ItemsSource = choices;
        _role.SelectedItem =
            choices.FirstOrDefault(choice => choice.Value == user.Role);

        foreach (var group in Groups)
        {
            _permissions[group.Label].IsChecked =
                group.Codes.All(user.Permissions.Contains);
        }

        SetEnabled();
    }

    private void SetEnabled()
    {
        var user = _users.SelectedItem as AdminExtrasClient.AccessUser;

        bool editable =
            user is not null &&
            !user.IsOwner &&
            user.Uid != Auth.Uid;

        _role.IsEnabled = editable;

        foreach (var check in _permissions.Values)
        {
            check.IsEnabled =
                editable &&
                !string.IsNullOrEmpty(user!.Uid) &&
                (_role.SelectedItem as Choice)?.Value == "Admin";
        }
    }
}

public sealed class BackupRestorePage : SettingsPage
{
    public BackupRestorePage() : base("Backups")
    {
        Action("Export JSON backup", async () =>
        {
            if (Settings.Current.ConfirmSensitiveActions)
            {
                bool proceed = await DisplayAlertAsync(
                    L.T("Confirm"),
                    L.T("Export school records to a file?"),
                    L.T("Continue"),
                    L.T("Cancel"));

                if (!proceed)
                    return;
            }

            var path = await Client.ExportAsync();

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = L.T("Export JSON backup"),
                File = new ShareFile(path)
            });

            SettingsAuditStore.Log(Auth.Uid, "Backup exported");
        });
    }
}

public sealed class AuditLogsPage : SettingsPage
{
    private readonly VerticalStackLayout _entries = new()
    {
        Spacing = 10
    };

    public AuditLogsPage() : base("Audit Logs")
    {
        Text("These are local settings activity records for your signed-in account.");

        Action("Refresh", () =>
        {
            Load();
            return Task.CompletedTask;
        });

        Action("Clear Local Logs", async () =>
        {
            bool proceed = await DisplayAlertAsync(
                L.T("Confirm"),
                L.T("Clear your local activity records?"),
                L.T("Clear"),
                L.T("Cancel"));

            if (proceed)
            {
                SettingsAuditStore.Clear(Auth.Uid);
                Load();
            }
        });

        Body.Add(_entries);
        Load();
    }

    private void Load()
    {
        _entries.Clear();

        foreach (var item in SettingsAuditStore.Read(Auth.Uid))
        {
            _entries.Add(new Label
            {
                Text = $"{item.Time.ToLocalTime():g} | {L.T(item.Action)}"
            });
        }
    }
}