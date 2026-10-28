using System;
using System.Globalization;
using System.Threading;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Pages;

public partial class LanguagePage : ContentPage
{
    public LanguagePage()
    {
        InitializeComponent();
    }

    private async void LanguageSelected(object sender, EventArgs e)
    {
        if (sender is not Button b)
            return;

        string language = b.Text;

        string cultureName = language switch
        {
            "English" => "en-ZA",
            "Sepedi" => "nso-ZA",
            "Setswana" => "tn-ZA",
            _ => "en-ZA"
        };

        // Save the selected language.
        Preferences.Default.Set("app_language", language);
        Preferences.Default.Set("app_culture", cultureName);

        // Apply the culture to the current app.
        var culture = new CultureInfo(cultureName);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        //English button also proceeds directly to the next onboarding page.
        // Navigate to the onboarding page.
        await Navigation.PushAsync(new OnboardingPage1());
    }
}

// References:
// [1] Microsoft Learn - .NET MAUI Preferences - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/preferences [Accessed 2026]
