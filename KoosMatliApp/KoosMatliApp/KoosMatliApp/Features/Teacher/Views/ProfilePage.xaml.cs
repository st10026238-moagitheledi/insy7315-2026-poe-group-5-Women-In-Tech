using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileHubViewModel?
        _viewModel;


    public ProfilePage()
    {
        InitializeComponent();


        HandlerChanged +=
            OnHandlerChanged;
    }


    private void OnHandlerChanged(
        object? sender,
        EventArgs e)
    {
        ResolveViewModel();
    }


    private void ResolveViewModel()
    {
        if (_viewModel != null)
        {
            return;
        }


        var services =
            Handler?
                .MauiContext?
                .Services;


        if (services == null)
        {
            return; // (Microsoft, 2026)
        }


        _viewModel =
            services.GetRequiredService
                <ProfileHubViewModel>();


        BindingContext =
            _viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        var success =
            await _viewModel
                .RefreshProfileAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Profile",
                _viewModel.ErrorMessage,
                "OK");
        }
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
                "Cancel"); // (Microsoft, 2026)


        if (!confirmed)
        {
            return;
        }


        var services =
            Handler?
                .MauiContext?
                .Services;


        var authService =
            services?
                .GetService
                    <FirebaseAuthService>();


        if (authService != null)
        {
            await authService
                .SignOutAsync();
        }


        await DisplayAlert(
            "Logged Out",
            "You have been logged out successfully.",
            "OK");
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
