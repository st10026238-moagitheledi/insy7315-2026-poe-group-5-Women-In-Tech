using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Features.Teacher.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class TeacherHomePage : ContentPage
{
    private TeacherHomeViewModel?
        _viewModel;


    public TeacherHomePage()
    {
        InitializeComponent();


        HandlerChanged +=
            OnHandlerChanged;
    }


    private void OnHandlerChanged(
        object? sender,
        EventArgs e)
    {
        ResolveViewModel(); // (Microsoft, 2026)
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


        if (services == null) // (Microsoft, 2026)
        {
            return;
        }


        _viewModel =
            services.GetRequiredService
                <TeacherHomeViewModel>();


        BindingContext =
            _viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing(); // (Microsoft, 2026)


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }

        await _viewModel
            .RefreshScheduleAsync();


        await _viewModel
            .RefreshAnnouncementAsync();
    }


    private async void OnViewAllClassesTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherClasses"); // (Microsoft, 2026)
    }


    private async void OnViewAllEventsTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherSchedule");
    }


    private async void OnTakeAttendanceTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherClasses");
    }


    private async void OnCreateAnnouncementTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddAnnouncementPage)); // (Microsoft, 2026)
    }


    private async void OnUploadHomeworkTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherClasses");
    }


    private async void OnViewTimetableTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherSchedule");
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
