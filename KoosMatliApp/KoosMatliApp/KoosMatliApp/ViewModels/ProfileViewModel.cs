using KoosMatliApp.Services;
namespace KoosMatliApp.ViewModels;
public class ProfileViewModel : BaseViewModel
{
    public ProfileViewModel() { Title = L.T("Profile"); }
    public string FullName => App.Services!.GetRequiredService<SettingsService>().Current.DisplayName;
    public string Email => App.Services!.GetRequiredService<AdminSession>().Email;
    public string Role => L.T("Administrator");
}
