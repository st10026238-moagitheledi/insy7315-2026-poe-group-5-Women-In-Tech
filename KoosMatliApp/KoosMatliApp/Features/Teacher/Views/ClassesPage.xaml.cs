using Microsoft.Extensions.DependencyInjection;

using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassesPage : ContentPage
{
    private ClassesViewModel?
        _viewModel;


    public ClassesPage()
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
                <ClassesViewModel>();


        BindingContext =
            _viewModel; // (Microsoft, 2026)
    }


    protected override async void
        OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        var success =
            await _viewModel
                .RefreshClassesAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Classes",
                _viewModel.ErrorMessage,
                "OK");
        }
    } // (Microsoft, 2026)


    private async void OnClassTapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext
                is TeacherClassItem selectedClass)
        {
            var navigationParameters =
                new Dictionary
                    <string, object>
                {
                    {
                        "SelectedClass",
                        selectedClass
                    }
                };


            await Shell.Current.GoToAsync(
                nameof(ClassDetailsPage),
                navigationParameters);
        } // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

