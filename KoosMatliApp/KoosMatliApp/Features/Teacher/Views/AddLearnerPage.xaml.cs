using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddLearnerPage : ContentPage, IQueryAttributable
{
    private readonly AddLearnerViewModel _viewModel;

    public AddLearnerPage()
    {
        InitializeComponent();

        _viewModel =
            new AddLearnerViewModel();

        BindingContext = _viewModel;
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


        _viewModel.LoadClassInformation(
            grade,
            subject);
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnAddLearnerClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                _viewModel.FirstName))
        {
            await DisplayAlert(
                "Learner",
                "Please enter the learner's first name.",
                "OK");

            return;
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
        }


        _viewModel.SaveLearner();


        await DisplayAlert(
            "Learner",
            "Learner has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}