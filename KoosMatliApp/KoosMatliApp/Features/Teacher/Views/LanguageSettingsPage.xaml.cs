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
    }


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