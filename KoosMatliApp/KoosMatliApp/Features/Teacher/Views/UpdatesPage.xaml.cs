using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class UpdatesPage : ContentPage
{
    private readonly UpdatesViewModel _viewModel;

    public UpdatesPage()
    {
        InitializeComponent();

        _viewModel =
            new UpdatesViewModel();

        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshAnnouncements();
    }


    private async void OnCreateAnnouncementClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddAnnouncementPage));
    }


    private async void OnAnnouncementTapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is ClassAnnouncementItem announcement)
        {
            var parameters =
                new Dictionary<string, object>
                {
                    {
                        "Announcement",
                        announcement
                    }
                };


            await Shell.Current.GoToAsync(
                nameof(AnnouncementDetailsPage),
                parameters);
        }
    }
}