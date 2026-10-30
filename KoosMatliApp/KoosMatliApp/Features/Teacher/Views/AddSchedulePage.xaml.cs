using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class AddSchedulePage : ContentPage
{
    private readonly AddScheduleViewModel _viewModel;

    public AddSchedulePage()
    {
        InitializeComponent();

        _viewModel =
            new AddScheduleViewModel();

        BindingContext =
            _viewModel;
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }


    private async void OnSaveScheduleClicked(
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


        _viewModel.SaveScheduleItem();


        await DisplayAlert(
            "Schedule",
            "Schedule item has been added successfully.",
            "OK");


        await Shell.Current.GoToAsync("..");
    }
}