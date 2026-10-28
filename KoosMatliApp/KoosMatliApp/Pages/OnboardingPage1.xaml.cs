using Microsoft.Maui.Controls;
using System.Globalization;
using KoosMatliApp.Resources.Localization;

namespace KoosMatliApp.Pages;

public partial class OnboardingPage1 : ContentPage
{
    public OnboardingPage1()
    {
        InitializeComponent();
    }

    private void ChangeLanguage_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);

            // Set thread cultures for active background translation mapping rules
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Direct localization files to load fresh string variations
            AppResources.Culture = culture;

            // Instantly recreate and reload the screen to swap text strings live
            Application.Current!.MainPage = new NavigationPage(new OnboardingPage1());
        }
    }

    private void Skip_Clicked(object sender, EventArgs e)
    {
        // finish onboarding and go to Login page
        Application.Current.MainPage = new Microsoft.Maui.Controls.NavigationPage(new Login());
    }

    // Method to call whenever a user selects a language button/dropdown
    private void SetAppLanguage(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);

        // Update thread cultures for background logic
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        // Force .NET MAUI localization manager to respect the new culture
        AppResources.Culture = culture;

        // Force the page to redraw with the fresh culture strings
        Application.Current.MainPage = new NavigationPage(new OnboardingPage1());
    }

    private async void Next_Clicked(object sender, EventArgs e)
    {
        var next = new OnboardingPage2();
        next.Opacity = 0;
        await Navigation.PushAsync(next, false);
        await next.FadeTo(1, 300);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // simple entrance animation
        OnboardFrame.Opacity = 0;
        OnboardImage.Scale = 0.8;
        await OnboardFrame.FadeTo(1, 400);
        await OnboardImage.ScaleTo(1, 600, Easing.SpringOut);
    }
}

// References and flow mapping:
// Microsoft Learn - .NET MAUI Navigation - https://learn.microsoft.com/dotnet/maui/fundamentals/navigation [Accessed 2026]
// UX - User Onboarding best practices - Nielsen Norman Group - https://www.nngroup.com/articles/user-onboarding/ [Accessed 2026]
//Microsoft Learn, 2026. Custom animation triggers inside .NET MAUI views. [online] Available at: <https://learn.microsoft.com/en-us/dotnet/maui/user-interface/animation/custom?view=net-maui-10.0> [Accessed 27 October 2026].