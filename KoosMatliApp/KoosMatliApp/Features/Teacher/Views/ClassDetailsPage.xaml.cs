using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassDetailsPage : ContentPage, IQueryAttributable
{
    private readonly ClassDetailsViewModel _viewModel;

    public ClassDetailsPage()
    {
        InitializeComponent();

        _viewModel = new ClassDetailsViewModel();

        BindingContext = _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshHomework();

        _viewModel.RefreshLearners();

        _viewModel.RefreshAnnouncements();
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "SelectedClass",
                out var selectedClass) &&
            selectedClass is TeacherClassItem classItem)
        {
            _viewModel.LoadClass(classItem);
        }
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnViewAllHomeworkTapped(
        object sender,
        TappedEventArgs e)
    {
        var parameters =
            new Dictionary<string, object>
            {
                { "Grade", _viewModel.Grade },
                { "Subject", _viewModel.Subject }
            };

        await Shell.Current.GoToAsync(
            nameof(HomeworkPage),
            parameters);
    }


    private async void OnResourceOpenClicked(
        object sender,
        EventArgs e)
    {
        if (sender is Button button &&
            button.BindingContext is ClassResourceItem resource)
        {
            await DisplayAlert(
                resource.Title,
                $"{resource.ResourceType}\n\n{resource.Description}",
                "Close");
        }
    }


    private void OnPresentClicked(
        object sender,
        EventArgs e)
    {
        if (sender is Button button &&
            button.BindingContext is LearnerItem learner)
        {
            learner.AttendanceStatus = "Present";
        }
    }


    private void OnAbsentClicked(
        object sender,
        EventArgs e)
    {
        if (sender is Button button &&
            button.BindingContext is LearnerItem learner)
        {
            learner.AttendanceStatus = "Absent";
        }
    }


    private async void OnSaveAttendanceClicked(
        object sender,
        EventArgs e)
    {
        await DisplayAlert(
            "Attendance",
            "Attendance has been recorded for this session.",
            "OK");
    }

    private async void OnAddLearnerClicked(
        object sender,
        EventArgs e)
    {
        var parameters =
            new Dictionary<string, object>
            {
            {
                "Grade",
                _viewModel.Grade
            },

            {
                "Subject",
                _viewModel.Subject
            }
            };


        await Shell.Current.GoToAsync(
            nameof(AddLearnerPage),
            parameters);
    }

    private async void OnLearnerTapped(
       object sender,
       TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is LearnerItem learner)
        {
            var parameters =
                new Dictionary<string, object>
                {
                {
                    "Grade",
                    _viewModel.Grade
                },

                {
                    "Subject",
                    _viewModel.Subject
                },

                {
                    "Learner",
                    learner
                }
                };


            await Shell.Current.GoToAsync(
                nameof(LearnerDetailsPage),
                parameters);
        }
    }

    private async void OnViewAllAnnouncementsTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherUpdates");
    }
}

