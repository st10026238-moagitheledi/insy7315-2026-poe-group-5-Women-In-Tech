using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class TeacherSettingsPage : ContentPage
{
    private readonly TeacherSettingsViewModel _viewModel;


    public TeacherSettingsPage()
    {
        InitializeComponent();


        _viewModel =
            new TeacherSettingsViewModel();


        BindingContext =
            _viewModel;
    } // (Microsoft, 2026)


    protected override void OnAppearing()
    {
        base.OnAppearing();


        _viewModel.LoadSettings();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnSaveSettingsClicked(
        object sender,
        EventArgs e)
    {
        _viewModel.SaveSettings();


        await DisplayAlert(
            "Settings",
            "Your settings have been saved.",
            "OK");
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
