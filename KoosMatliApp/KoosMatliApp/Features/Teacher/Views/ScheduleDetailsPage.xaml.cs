using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ScheduleDetailsPage
    : ContentPage, IQueryAttributable
{
    private ScheduleDetailsViewModel?
        _viewModel;

    private TeacherScheduleItem?
        _scheduleItem;


    public ScheduleDetailsPage()
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
                <ScheduleDetailsViewModel>();


        BindingContext =
            _viewModel;


        if (_scheduleItem != null)
        {
            _viewModel.LoadScheduleItem(
                _scheduleItem);
        } // (Microsoft, 2026)
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "ScheduleItem",
                out var itemValue) &&
            itemValue
                is TeacherScheduleItem item)
        {
            _scheduleItem =
                item;


            _viewModel?
                .LoadScheduleItem(
                    item);
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
        } // (Microsoft, 2026)


        if (string.IsNullOrWhiteSpace(
                _viewModel.Title))
        {
            await DisplayAlert(
                "Schedule",
                "Please enter a title.",
                "OK");

            return;
        }


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
                .SaveChangesAsync();


        if (!success)
        {
            await DisplayAlert(
                "Schedule",
                _viewModel.ErrorMessage,
                "OK");

            return;
        } // (Microsoft, 2026)


        await DisplayAlert(
            "Schedule",
            "Schedule item has been updated.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnDeleteScheduleClicked(
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
                "Delete Schedule Item",
                "Are you sure you want to delete this schedule item?",
                "Delete",
                "Cancel");


        if (!confirmed)
        {
            return;
        } // (Microsoft, 2026)


        var success =
            await _viewModel
                .DeleteScheduleItemAsync();


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
            "Schedule item has been deleted.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
