using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class PersonalInformationPage : ContentPage
{
    private PersonalInformationViewModel?
        _viewModel;


    public PersonalInformationPage()
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
                <PersonalInformationViewModel>();


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
        } // (Microsoft, 2026)


        var success =
            await _viewModel
                .LoadProfileAsync();


        if (!success &&
            !string.IsNullOrWhiteSpace(
                _viewModel.ErrorMessage))
        {
            await DisplayAlert(
                "Profile",
                _viewModel.ErrorMessage,
                "OK");
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
                _viewModel.FirstName) ||
            string.IsNullOrWhiteSpace(
                _viewModel.LastName))
        {
            await DisplayAlert(
                "Profile",
                "Please enter your first and last name.",
                "OK");

            return;
        }


        var success =
            await _viewModel
                .SaveProfileAsync();


        if (!success)
        {
            await DisplayAlert(
                "Profile",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Profile",
            "Your personal information has been updated.",
            "OK");
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].


