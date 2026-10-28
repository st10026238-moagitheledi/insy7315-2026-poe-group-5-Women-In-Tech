using KoosMatliApp.Resources.Localization;
using Microsoft.Maui.Controls;
using System.Globalization;

namespace KoosMatliApp.Pages;

public partial class OnboardingPage3 : ContentPage
{
    public OnboardingPage3()
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
        Application.Current.MainPage = new Microsoft.Maui.Controls.NavigationPage(new Login());
    }

    private void Next_Clicked(object sender, EventArgs e)
    {
        var next = new OnboardingPage4();
        next.Opacity = 0;
        Navigation.PushAsync(next, false);
        _ = next.FadeTo(1, 300);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        OnboardFrame.Opacity = 0;
        OnboardImage.Scale = 0.8;
        await OnboardFrame.FadeTo(1, 400);
        await OnboardImage.ScaleTo(1, 600, Easing.SpringOut);
    }
}

// References and flow mapping:
// [1] Microsoft Learn - .NET MAUI Navigation - https://learn.microsoft.com/dotnet/maui/fundamentals/navigation [Accessed 2026]
// [2] UX - User Onboarding best practices - Nielsen Norman Group - https://www.nngroup.com/articles/user-onboarding/ [Accessed 2026]
// Internal flow: Onboarding -> GetStarted -> Login -> (SignUp | ForgotPassword | ResetPassword) -> Loading -> Staff
//Microsoft Learn, 2026. Custom animation triggers inside .NET MAUI views. [online] Available at: <https://learn.microsoft.com/en-us/dotnet/maui/user-interface/animation/custom?view=net-maui-10.0> [Accessed 27 October 2026].