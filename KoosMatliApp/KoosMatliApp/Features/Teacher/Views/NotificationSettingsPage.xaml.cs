using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class NotificationSettingsPage : ContentPage
{
    private readonly NotificationSettingsViewModel _viewModel;


    public NotificationSettingsPage()
    {
        InitializeComponent();

        _viewModel =
            new NotificationSettingsViewModel();

        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.LoadSettings();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnSaveSettingsClicked(
        object sender,
        EventArgs e)
    {
        _viewModel.SaveSettings();


        await DisplayAlert(
            "Notifications",
            "Notification settings have been saved.",
            "OK");
    }
}