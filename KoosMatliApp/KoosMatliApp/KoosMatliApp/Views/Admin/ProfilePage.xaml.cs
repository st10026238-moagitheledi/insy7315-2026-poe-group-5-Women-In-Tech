using KoosMatliApp.Services;
namespace KoosMatliApp.Views.Admin;
public partial class ProfilePage : ContentPage
{
    public ProfilePage() { InitializeComponent(); }
    protected override void OnAppearing()
    { //(Microsoft Learn, 2026a).
        base.OnAppearing(); var settings = App.Services!.GetRequiredService<SettingsService>();
        var auth = App.Services!.GetRequiredService<AdminSession>(); //(Microsoft Learn, 2026a).
        AdminNameLabel.Text = string.IsNullOrWhiteSpace(settings.Current.DisplayName) ? auth.Email : settings.Current.DisplayName;
        AdminEmailLabel.Text = auth.Email;
    }
    private async void AccountSettings_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-account");
    private async void RolesPermissions_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-roles");
    private async void Security_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-security");
    private async void BackupRestore_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-backup"); //(Microsoft Learn, 2026b).
    private async void AuditLogs_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-audit");
    private async void SystemSettings_Clicked(object sender, EventArgs e) => await Shell.Current.GoToAsync("settings-system");
}
/*Reference List
 
Microsoft Corporation, "Page Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle

Microsoft Corporation, "Shell Navigation in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation
 */