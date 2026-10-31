using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddAnnouncementPage : ContentPage
{
    private AddAnnouncementViewModel?
        _viewModel;


    public AddAnnouncementPage()
    {
        InitializeComponent();

        HandlerChanged +=
            OnHandlerChanged;
    } // (Microsoft, 2026)


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
                <AddAnnouncementViewModel>();


        BindingContext =
            _viewModel;
    } // (Microsoft, 2026)


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnPublishAnnouncementClicked(
        object sender,
        EventArgs e)
    {
        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.Title))
        {
            await DisplayAlert(
                "Announcement",
                "Please enter a title.",
                "OK");

            return;
        } // (Microsoft, 2026)


        if (string.IsNullOrWhiteSpace(
                _viewModel.Message))
        {
            await DisplayAlert(
                "Announcement",
                "Please enter a message.",
                "OK");

            return;
        }


        var success =
            await _viewModel
                .SaveAnnouncementAsync();


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
            "Announcement has been published successfully.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

