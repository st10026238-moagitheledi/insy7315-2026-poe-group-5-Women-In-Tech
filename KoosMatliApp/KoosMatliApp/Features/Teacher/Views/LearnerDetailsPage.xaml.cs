using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class LearnerDetailsPage : ContentPage, IQueryAttributable
{
    private readonly LearnerDetailsViewModel _viewModel;

    public LearnerDetailsPage()
    {
        InitializeComponent();

        _viewModel =
            new LearnerDetailsViewModel();

        BindingContext =
            _viewModel;
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        var grade =
            query.TryGetValue(
                "Grade",
                out var gradeValue)
            ? gradeValue?.ToString() ?? string.Empty
            : string.Empty;


        var subject =
            query.TryGetValue(
                "Subject",
                out var subjectValue)
            ? subjectValue?.ToString() ?? string.Empty
            : string.Empty;


        if (query.TryGetValue(
                "Learner",
                out var learnerValue) &&
            learnerValue is LearnerItem learner)
        {
            _viewModel.LoadLearner(
                grade,
                subject,
                learner);
        }
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnSaveChangesClicked(
        object sender,
        EventArgs e)
    {
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
        }


        _viewModel.SaveChanges();


        await DisplayAlert(
            "Learner",
            "Learner details have been updated.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }


    private async void OnDeleteLearnerClicked(
        object sender,
        EventArgs e)
    {
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


        _viewModel.DeleteLearner();


        await DisplayAlert(
            "Learner",
            "Learner has been removed from the class.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}