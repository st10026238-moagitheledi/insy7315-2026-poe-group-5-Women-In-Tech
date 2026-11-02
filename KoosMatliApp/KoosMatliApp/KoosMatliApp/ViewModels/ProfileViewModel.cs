using KoosMatliApp.Services;
namespace KoosMatliApp.ViewModels;
public class ProfileViewModel : BaseViewModel //(Microsoft Learn, 2026a).
{
    public ProfileViewModel() { Title = L.T("Profile"); }
    public string FullName => App.Services!.GetRequiredService<SettingsService>().Current.DisplayName; //(Microsoft Learn, 2026a).
    public string Email => App.Services!.GetRequiredService<AdminSession>().Email;
    public string Role => L.T("Administrator"); //(Microsoft Learn, 2026b).
}

/*Reference List
 
Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm

Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

Microsoft Corporation, "Globalize and Localize .NET Applications," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/core/extensions/localization
 */