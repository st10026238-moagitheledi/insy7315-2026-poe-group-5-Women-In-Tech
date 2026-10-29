using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class TeacherHomePage : ContentPage
{
    public TeacherHomePage()
    {
        InitializeComponent();

        BindingContext = new TeacherHomeViewModel();
    }


    private async void OnViewAllClassesTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherClasses");
    }


    private async void OnViewAllEventsTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherSchedule");
    }


    private async void OnTakeAttendanceTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherClasses");
    }


    private async void OnCreateAnnouncementTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherUpdates");
    }


    private async void OnUploadHomeworkTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherClasses");
    }


    private async void OnViewTimetableTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//TeacherSchedule");
    }
}