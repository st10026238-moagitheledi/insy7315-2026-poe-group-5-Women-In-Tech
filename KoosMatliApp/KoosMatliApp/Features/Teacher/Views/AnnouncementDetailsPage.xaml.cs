using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AnnouncementDetailsPage
    : ContentPage, IQueryAttributable
{
    private AnnouncementDetailsViewModel?
        _viewModel;

    private ClassAnnouncementItem?
        _announcement;


    public AnnouncementDetailsPage()
    {
        InitializeComponent();

        HandlerChanged +=
            OnHandlerChanged; // (Microsoft, 2026)
    }


    private void OnHandlerChanged(
        object? sender,
        EventArgs e)
    {
        ResolveViewModel();
    }


    private void ResolveViewModel()
    {
        if (_viewModel != null)
        {
            return;
        }


        var services =
            Handler?
                .MauiContext?
                .Services;


        if (services == null)
        {
            return;
        }


        _viewModel =
            services.GetRequiredService
                <AnnouncementDetailsViewModel>();


        BindingContext =
            _viewModel;


        if (_announcement != null)
        {
            _viewModel.LoadAnnouncement(
                _announcement);
        }
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "Announcement",
                out var announcementValue) &&
            announcementValue
                is ClassAnnouncementItem announcement)
        {
            _announcement =
                announcement;


            _viewModel?
                .LoadAnnouncement(
                    announcement); // (Microsoft, 2026)
        }
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnSaveChangesClicked(
        object sender,
        EventArgs e)
    {
        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


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


        var success =
            await _viewModel
                .SaveChangesAsync();


        if (!success)
        {
            await DisplayAlert(
                "Announcement",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Announcement",
            "Announcement has been updated.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnDeleteAnnouncementClicked(
        object sender,
        EventArgs e)
    {
        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


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


        var success =
            await _viewModel
                .DeleteAnnouncementAsync();


        if (!success)
        {
            await DisplayAlert(
                "Announcement",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Announcement",
            "Announcement has been deleted.",
            "OK");


        await Shell.Current.GoToAsync( // (Microsoft, 2026)
            "..");
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

