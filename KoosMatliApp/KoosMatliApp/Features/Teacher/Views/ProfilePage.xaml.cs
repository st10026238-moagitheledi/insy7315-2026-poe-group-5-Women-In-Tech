using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileHubViewModel _viewModel;


    public ProfilePage()
    {
        InitializeComponent();


        _viewModel =
            new ProfileHubViewModel();


        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();


        _viewModel.RefreshProfile();
    }


    private async void OnPersonalInformationTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(PersonalInformationPage));
    }


    private async void OnChangePasswordTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(ChangePasswordPage));
    }


    private async void OnLanguageTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(LanguageSettingsPage));
    }


    private async void OnNotificationSettingsTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(NotificationSettingsPage));
    }


    private async void OnHelpSupportTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(HelpSupportPage));
    }


    private async void OnAboutAppTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AboutAppPage));
    }


    private async void OnLogoutClicked(
        object sender,
        EventArgs e)
    {
        var confirmed =
            await DisplayAlert(
                "Log Out",
                "Are you sure you want to log out?",
                "Log Out",
                "Cancel");


        if (!confirmed)
        {
            return;
        }


        await DisplayAlert(
            "Log Out",
            "Firebase Authentication will handle the actual logout when the backend is connected.",
            "OK");
    }
}