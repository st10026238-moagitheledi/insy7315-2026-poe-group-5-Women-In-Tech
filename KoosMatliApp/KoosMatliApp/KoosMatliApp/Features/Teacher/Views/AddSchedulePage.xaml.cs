using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddSchedulePage : ContentPage
{
    private AddScheduleViewModel?
        _viewModel;


    public AddSchedulePage()
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
            return; // (Microsoft, 2026)
        }


        _viewModel =
            services.GetRequiredService
                <AddScheduleViewModel>();


        BindingContext =
            _viewModel;
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnSaveScheduleClicked(
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
                "Schedule",
                "Please enter a title.",
                "OK");

            return;
        } // (Microsoft, 2026)


        if (_viewModel.EndTime <=
            _viewModel.StartTime)
        {
            await DisplayAlert(
                "Schedule",
                "End time must be later than start time.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.Location))
        {
            await DisplayAlert(
                "Schedule",
                "Please enter a location.",
                "OK");

            return;
        }


        var success =
            await _viewModel
                .SaveScheduleItemAsync();


        if (!success)
        {
            await DisplayAlert(
                "Schedule",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Schedule",
            "Schedule item has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync(
            ".."); // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

