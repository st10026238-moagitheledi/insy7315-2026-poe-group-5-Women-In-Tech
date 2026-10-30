using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AnnouncementDetailsPage : ContentPage, IQueryAttributable
{
    private readonly AnnouncementDetailsViewModel _viewModel;

    public AnnouncementDetailsPage()
    {
        InitializeComponent();

        _viewModel =
            new AnnouncementDetailsViewModel();

        BindingContext =
            _viewModel;
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "Announcement",
                out var announcementValue) &&
            announcementValue is ClassAnnouncementItem announcement)
        {
            _viewModel.LoadAnnouncement(
                announcement);
        }
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnSaveChangesClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                _viewModel.Title) ||
            string.IsNullOrWhiteSpace(
                _viewModel.Message))
        {
            await DisplayAlert(
                "Announcement",
                "Please complete the title and message.",
                "OK");

            return;
        }


        _viewModel.SaveChanges();


        await DisplayAlert(
            "Announcement",
            "Announcement has been updated.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }


    private async void OnDeleteAnnouncementClicked(
        object sender,
        EventArgs e)
    {
        var confirmed =
            await DisplayAlert(
                "Delete Announcement",
                "Are you sure you want to delete this announcement?",
                "Delete",
                "Cancel");


        if (!confirmed)
        {
            return;
        }


        _viewModel.DeleteAnnouncement();


        await DisplayAlert(
            "Announcement",
            "Announcement has been deleted.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}