using Microsoft.Extensions.DependencyInjection;

using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class LearnerDetailsPage
    : ContentPage, IQueryAttributable
{
    private LearnerDetailsViewModel?
        _viewModel;


    private string _grade =
        string.Empty; // (Microsoft, 2026)

    private string _subject =
        string.Empty;

    private string _classId =
        string.Empty;

    private LearnerItem?
        _learner;


    public LearnerDetailsPage()
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
        }


        _viewModel =
            services.GetRequiredService
                <LearnerDetailsViewModel>();


        BindingContext =
            _viewModel;


        if (_learner != null)
        {
            _viewModel.LoadLearner(
                _grade,
                _subject,
                _classId,
                _learner);
        }
    } // (Microsoft, 2026)


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
                : string.Empty;


        if (query.TryGetValue(
                "Learner",
                out var learnerValue) &&
            learnerValue is LearnerItem learner)
        {
            _learner =
                learner;


            _viewModel?
                .LoadLearner(
                    _grade,
                    _subject,
                    _classId,
                    learner);
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
        if (_viewModel == null)
        {
            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.FirstName) ||
            string.IsNullOrWhiteSpace(
                _viewModel.LastName))
        {
            await DisplayAlert(
                "Learner",
                "Please enter the learner's full name.",
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
                .SaveChangesAsync();


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
            "Learner details have been updated.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnDeleteLearnerClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        var confirmed =
            await DisplayAlert(
                "Delete Learner",
                "Are you sure you want to remove this learner from the class?",
                "Delete",
                "Cancel");


        if (!confirmed)
        {
            return;
        }
        // (Microsoft, 2026)

        var success =
            await _viewModel
                .DeleteLearnerAsync();


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
            "Learner has been removed from the class.",
            "OK");


        await Shell.Current.GoToAsync(
            "..");
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
