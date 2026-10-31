using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ClassDetailsPage
    : ContentPage, IQueryAttributable
{
    private ClassDetailsViewModel?
        _viewModel;

    private TeacherClassItem?
        _selectedClass;


    public ClassDetailsPage()
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
                .Services; // (Microsoft, 2026)


        if (services == null)
        {
            return;
        }


        _viewModel =
            services.GetRequiredService
                <ClassDetailsViewModel>();


        BindingContext =
            _viewModel;


        if (_selectedClass != null)
        {
            _viewModel.LoadClass(
                _selectedClass);
        }
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "SelectedClass",
                out var selectedClass) &&
            selectedClass
                is TeacherClassItem classItem)
        {
            _selectedClass =
                classItem;


            _viewModel?
                .LoadClass(
                    classItem);
        }
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();


        ResolveViewModel();


        if (_viewModel == null)
        {
            return;
        }


        await _viewModel
            .RefreshHomeworkAsync();


        await _viewModel
            .RefreshAnnouncementsAsync();


        await _viewModel
            .RefreshTimetableAsync();


        var learnerSuccess =
            await _viewModel
                .RefreshLearnersAsync();


        if (!learnerSuccess &&
            !string.IsNullOrWhiteSpace(
                _viewModel.LearnerErrorMessage))
        {
            await DisplayAlert(
                "Learners",
                _viewModel.LearnerErrorMessage,
                "OK");

            return; // (Microsoft, 2026)
        }


        await _viewModel
            .RefreshAttendanceAsync();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private async void OnViewFullTimetableTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherSchedule");
    }


    private async void OnViewAllHomeworkTapped(
        object sender,
        TappedEventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


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
            nameof(HomeworkPage),
            parameters);
    }


    private async void OnResourceOpenClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.BindingContext
                is not ClassResourceItem resource)
        {
            return;
        }


        if (string.IsNullOrWhiteSpace(
                resource.FilePath))
        {
            await DisplayAlert(
                resource.Title,
                "No file is currently attached to this resource.",
                "OK");

            return;
        }


        if (!File.Exists(
                resource.FilePath))
        {
            await DisplayAlert(
                "File Not Found",
                "The attached file could not be found on this device.",
                "OK");

            return;
        }


        try
        {
            await Launcher.Default.OpenAsync(
                new OpenFileRequest
                {
                    File =
                        new ReadOnlyFile(
                            resource.FilePath)
                });
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Unable to Open File",
                ex.Message,
                "OK"); // (Microsoft, 2026)
        }
    }


    private void OnPresentClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        if (sender is Button button &&
            button.BindingContext
                is LearnerItem learner)
        {
            learner.AttendanceStatus =
                "Present";


            _viewModel
                .UpdateAttendanceSummary();
        }
    }


    private void OnAbsentClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        if (sender is Button button &&
            button.BindingContext
                is LearnerItem learner)
        {
            learner.AttendanceStatus =
                "Absent";


            _viewModel
                .UpdateAttendanceSummary();
        }
    }


    private async void OnSaveAttendanceClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


        var success =
            await _viewModel
                .SaveAttendanceAsync();


        if (!success)
        {
            await DisplayAlert(
                "Attendance",
                _viewModel.AttendanceErrorMessage,
                "OK");

            return;
        }


        await DisplayAlert(
            "Attendance",
            "Attendance has been recorded successfully.",
            "OK");
    } // (Microsoft, 2026)


    private async void OnAddLearnerClicked(
        object sender,
        EventArgs e)
    {
        if (_viewModel == null)
        {
            return;
        }


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
                    "ClassId",
                    _viewModel.ClassId
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
        if (_viewModel == null)
        {
            return;
        }


        if (sender is Border border &&
            border.BindingContext
                is LearnerItem learner)
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
                        "ClassId",
                        _viewModel.ClassId
                    },

                    {
                        "Learner",
                        learner
                    }
                };


            await Shell.Current.GoToAsync(
                nameof(LearnerDetailsPage),
                parameters); // (Microsoft, 2026)
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


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

