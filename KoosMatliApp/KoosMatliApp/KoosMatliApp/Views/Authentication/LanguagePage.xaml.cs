using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Authentication;

public partial class LanguagePage : ContentPage
{
    private AppLanguage _selectedLanguage = AppLanguage.English;

    public LanguagePage()
    {
        InitializeComponent();

        _selectedLanguage = LocalizationService.Instance.Language;
        UpdateLanguageSelection();
    }

    private void OnEnglishSelected(object sender, TappedEventArgs e)
    {
        _selectedLanguage = AppLanguage.English;
        UpdateLanguageSelection();
    }

    private void OnSepediSelected(object sender, TappedEventArgs e)
    {
        _selectedLanguage = AppLanguage.Sepedi;
        UpdateLanguageSelection();
    }

    private void OnSetswanaSelected(object sender, TappedEventArgs e)
    {
        _selectedLanguage = AppLanguage.Setswana;
        UpdateLanguageSelection();
    }

    private void UpdateLanguageSelection()
    {
        EnglishCard.Stroke = _selectedLanguage == AppLanguage.English
            ? Color.FromArgb("#FFBF19")
            : Color.FromArgb("#DCE3EF");

        SepediCard.Stroke = _selectedLanguage == AppLanguage.Sepedi
            ? Color.FromArgb("#FFBF19")
            : Color.FromArgb("#DCE3EF");

        SetswanaCard.Stroke = _selectedLanguage == AppLanguage.Setswana
            ? Color.FromArgb("#FFBF19")
            : Color.FromArgb("#DCE3EF");

        EnglishCard.StrokeThickness =
            _selectedLanguage == AppLanguage.English ? 2 : 1;

        SepediCard.StrokeThickness =
            _selectedLanguage == AppLanguage.Sepedi ? 2 : 1;

        SetswanaCard.StrokeThickness =
            _selectedLanguage == AppLanguage.Setswana ? 2 : 1;

        EnglishCheck.Text =
            _selectedLanguage == AppLanguage.English ? "✓" : "";

        SepediCheck.Text =
            _selectedLanguage == AppLanguage.Sepedi ? "✓" : "";

        SetswanaCheck.Text =
            _selectedLanguage == AppLanguage.Setswana ? "✓" : "";
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        LocalizationService.Instance.SetLanguage(_selectedLanguage);

        ContinueButton.IsEnabled = false;

        try
        {
            await Navigation.PushAsync(new OnboardingPage());
        }
        finally
        {
            ContinueButton.IsEnabled = true;
        }
    }
}
