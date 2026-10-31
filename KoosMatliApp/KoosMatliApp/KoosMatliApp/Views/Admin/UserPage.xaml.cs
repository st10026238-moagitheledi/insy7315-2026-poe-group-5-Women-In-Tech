using KoosMatliApp.ViewModels;
using KoosMatliApp.Models;

namespace KoosMatliApp.Views.Admin;

public partial class UserPage : ContentPage
{
    private bool _openingUser;

    public UserPage()
    {
        InitializeComponent();

        BindingContext = App.Services?.GetService(typeof(UsersViewModel))
            as UsersViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UsersViewModel vm)
            await vm.InitializeAsync();
    }

    private async void UserSelected(
        object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not User user)
            return;

        ((CollectionView)sender).SelectedItem = null;

        if (_openingUser)
            return;

        _openingUser = true;

        try
        {
            await Navigation.PushModalAsync(new UserDetailsPage(user));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                L.T("Error"), ex.Message, L.T("OK"));
        }
        finally
        {
            _openingUser = false;
        }
    }

    private async void AddNewUser_Clicked(object sender, EventArgs e) =>
        await Shell.Current.GoToAsync("addnewuser");

    private void ParentsFilter_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is UsersViewModel vm)
            vm.FilterByRole("Parent");
    }

    private void TeachersFilter_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is UsersViewModel vm)
            vm.FilterByRole("Teacher");
    }

    private void LearnersFilter_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is UsersViewModel vm)
            vm.FilterByRole("Learner");
    }
}