using Microsoft.Extensions.DependencyInjection;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddLearnerPage
    : ContentPage, IQueryAttributable
{
    private AddLearnerViewModel?
        _viewModel;


    private string _grade = string.Empty;

    private string _subject = string.Empty;

    private string _classId = string.Empty; // (Microsoft, 2026)


    public AddLearnerPage()
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
                <AddLearnerViewModel>();


        BindingContext =
            _viewModel;


        _viewModel.LoadClassInformation(
            _grade,
            _subject,
            _classId);
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        _grade =
            query.TryGetValue(
                "Grade",
                out var gradeValue)
                ? gradeValue?.ToString() ??
                    string.Empty
                : string.Empty;


        _subject =
            query.TryGetValue(
                "Subject",
                out var subjectValue)
                ? subjectValue?.ToString() ??
                    string.Empty
                : string.Empty;


        _classId =
            query.TryGetValue(
                "ClassId",
                out var classValue)
                ? classValue?.ToString() ??
                    string.Empty
                : string.Empty; // (Microsoft, 2026)


        _viewModel?
            .LoadClassInformation(
                _grade,
                _subject,
                _classId);
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnAddLearnerClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.FirstName))
        {
            await DisplayAlert(
                "Learner",
                "Please enter the learner's first name.",
                "OK");

            return; // (Microsoft, 2026)
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.LastName))
        {
            await DisplayAlert(
                "Learner",
                "Please enter the learner's last name.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.StudentNumber))
        {
            await DisplayAlert(
                "Learner",
                "Please enter the student number.",
                "OK");

            return;
        } // (Microsoft, 2026)


        var success =
            await _viewModel
                .SaveLearnerAsync();


        if (!success)
        {
            await DisplayAlert(
                "Learner",
                _viewModel.ErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Learner",
            "Learner has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    }
}

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

