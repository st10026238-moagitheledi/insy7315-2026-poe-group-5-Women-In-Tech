using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class UpdatesPage : ContentPage
{
    private UpdatesViewModel?
        _viewModel;


    public UpdatesPage()
    {
        InitializeComponent();

        HandlerChanged +=
            OnHandlerChanged;
    }


    private void OnHandlerChanged(
        object? sender,
        EventArgs e)
    {
        ResolveViewModel();
    } // (Microsoft, 2026)


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
                <UpdatesViewModel>();


        BindingContext =
            _viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return; // (Microsoft, 2026)
        }


        var success =
            await _viewModel
                .RefreshAnnouncementsAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Announcements",
                _viewModel.ErrorMessage,
                "OK");
        }
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
            border.BindingContext
                is ClassAnnouncementItem announcement)
        {
            var parameters =
                new Dictionary<string, object>
                {
                    {
                        "Announcement",
                        announcement
                    }
                }; // (Microsoft, 2026)


            await Shell.Current.GoToAsync(
                nameof(AnnouncementDetailsPage),
                parameters);
        }
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
