using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddAnnouncementPage : ContentPage
{
    private readonly AddAnnouncementViewModel _viewModel;

    public AddAnnouncementPage()
    {
        InitializeComponent();

        _viewModel =
            new AddAnnouncementViewModel();

        BindingContext =
            _viewModel;
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnPublishAnnouncementClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                _viewModel.Title))
        {
            await DisplayAlert(
                "Announcement",
                "Please enter a title.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.Message))
        {
            await DisplayAlert(
                "Announcement",
                "Please enter a message.",
                "OK");

            return;
        }


        _viewModel.SaveAnnouncement();


        await DisplayAlert(
            "Announcement",
            "Announcement has been published successfully.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}