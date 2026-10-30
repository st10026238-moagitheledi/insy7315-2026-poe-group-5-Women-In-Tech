using KoosMatliApp.Features.Teacher.Models;
using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class SchedulePage : ContentPage
{
    private readonly ScheduleViewModel _viewModel;

    public SchedulePage()
    {
        InitializeComponent();

        _viewModel =
            new ScheduleViewModel();

        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshSchedule();
    }


    private async void OnAddScheduleClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync(
            nameof(AddSchedulePage));
    }


    private async void OnScheduleItemTapped(
        object sender,
        TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is TeacherScheduleItem scheduleItem)
        {
            var parameters =
                new Dictionary<string, object>
                {
                    {
                        "ScheduleItem",
                        scheduleItem
                    }
                };


            await Shell.Current.GoToAsync(
                nameof(ScheduleDetailsPage),
                parameters);
        }
    }
}