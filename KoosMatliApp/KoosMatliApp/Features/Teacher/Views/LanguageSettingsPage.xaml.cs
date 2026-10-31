using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class LanguageSettingsPage : ContentPage
{
    private readonly LanguageSettingsViewModel _viewModel;


    public LanguageSettingsPage()
    {
        InitializeComponent();

        _viewModel =
            new LanguageSettingsViewModel();

        BindingContext =
            _viewModel;
    } // (Microsoft, 2026)


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.LoadLanguage();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
    // (Microsoft, 2026)

    private async void OnSaveLanguageClicked(
        object sender,
        EventArgs e)
    {
        _viewModel.SaveLanguage();


        await DisplayAlert(
            "Language",
            $"{_viewModel.SelectedLanguage} has been selected.",
            "OK");
    }
}

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

