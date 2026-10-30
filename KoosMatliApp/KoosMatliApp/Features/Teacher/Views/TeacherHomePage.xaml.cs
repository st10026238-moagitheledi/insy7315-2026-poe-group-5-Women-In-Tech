using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class TeacherHomePage : ContentPage
{
    private readonly TeacherHomeViewModel _viewModel;


    public TeacherHomePage()
    {
        InitializeComponent();


        _viewModel =
            new TeacherHomeViewModel();


        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();


        _viewModel.RefreshUpcomingEvents();

        _viewModel.RefreshAnnouncement();
    }


    private async void OnViewAllClassesTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "//TeacherClasses");
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
            nameof(AddAnnouncementPage));
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