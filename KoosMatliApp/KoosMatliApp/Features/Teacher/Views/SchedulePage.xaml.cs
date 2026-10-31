using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class SchedulePage : ContentPage
{
    private ScheduleViewModel?
        _viewModel;


    public SchedulePage()
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
        } // (Microsoft, 2026)


        _viewModel =
            services.GetRequiredService
                <ScheduleViewModel>();


        BindingContext =
            _viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        var success =
            await _viewModel
                .RefreshScheduleAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Schedule",
                _viewModel.ErrorMessage,
                "OK");
        } // (Microsoft, 2026)
    }


    private async void OnAddScheduleClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddSchedulePage));
    }


    private async void OnScheduleItemTapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext
                is TeacherScheduleItem scheduleItem)
        {
            var parameters =
                new Dictionary<string, object>
                {
                    {
                        "ScheduleItem",
                        scheduleItem
                    }
                };


            await Shell.Current.GoToAsync(
                nameof(ScheduleDetailsPage),
                parameters);
        } // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
