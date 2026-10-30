using KoosMatliApp.Features.Teacher.ViewModels;

namespace KoosMatliApp.Features.Teacher.Views;

public partial class PersonalInformationPage : ContentPage
{
    private readonly PersonalInformationViewModel _viewModel;


    public PersonalInformationPage()
    {
        InitializeComponent();

        _viewModel =
            new PersonalInformationViewModel();

        BindingContext =
            _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.LoadProfile();
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
        if (string.IsNullOrWhiteSpace(_viewModel.FirstName) ||
            string.IsNullOrWhiteSpace(_viewModel.LastName))
        {
            await DisplayAlert(
                "Profile",
                "Please enter your first and last name.",
                "OK");

            return;
        }


        if (string.IsNullOrWhiteSpace(_viewModel.Email))
        {
            await DisplayAlert(
                "Profile",
                "Please enter your email address.",
                "OK");

            return;
        }


        _viewModel.SaveProfile();


        await DisplayAlert(
            "Profile",
            "Your personal information has been updated.",
            "OK");
    }
}