using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class ScheduleDetailsPage : ContentPage, IQueryAttributable
{
    private readonly ScheduleDetailsViewModel _viewModel;

    public ScheduleDetailsPage()
    {
        InitializeComponent();

        _viewModel =
            new ScheduleDetailsViewModel();

        BindingContext =
            _viewModel;
    }


    public void ApplyQueryAttributes(
        IDictionary<string, object> query)
    {
        if (query.TryGetValue(
                "ScheduleItem",
                out var itemValue) &&
            itemValue is TeacherScheduleItem item)
        {
            _viewModel.LoadScheduleItem(
                item);
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
                _viewModel.Title))
        {
            await DisplayAlert(
                "Schedule",
                "Please enter a title.",
                "OK");

            return;
        }


        if (_viewModel.EndTime <=
            _viewModel.StartTime)
        {
            await DisplayAlert(
                "Schedule",
                "End time must be later than start time.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(
                _viewModel.Location))
        {
            await DisplayAlert(
                "Schedule",
                "Please enter a location.",
                "OK");

            return;
        }


        _viewModel.SaveChanges();


        await DisplayAlert(
            "Schedule",
            "Schedule item has been updated.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }


    private async void OnDeleteScheduleClicked(
        object sender,
        EventArgs e)
    {
        var confirmed =
            await DisplayAlert(
                "Delete Schedule Item",
                "Are you sure you want to delete this schedule item?",
                "Delete",
                "Cancel");


        if (!confirmed)
        {
            return;
        }


        _viewModel.DeleteScheduleItem();


        await DisplayAlert(
            "Schedule",
            "Schedule item has been deleted.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}