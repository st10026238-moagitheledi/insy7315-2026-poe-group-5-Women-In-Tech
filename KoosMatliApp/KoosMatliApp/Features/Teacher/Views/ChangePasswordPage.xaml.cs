namespace KoosMatliApp.Features.Teacher.Views;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(
            "..");
    }


    private void OnCurrentPasswordEyeClicked(
        object sender,
        EventArgs e)
    {
        CurrentPasswordEntry.IsPassword =
            !CurrentPasswordEntry.IsPassword;
    }


    private void OnNewPasswordEyeClicked(
        object sender,
        EventArgs e)
    {
        NewPasswordEntry.IsPassword =
            !NewPasswordEntry.IsPassword;
    }


    private void OnConfirmPasswordEyeClicked(
        object sender,
        EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword =
            !ConfirmPasswordEntry.IsPassword;
    }


    private async void OnChangePasswordClicked(
        object sender,
        EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(
                CurrentPasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(
                NewPasswordEntry.Text) ||
            string.IsNullOrWhiteSpace(
                ConfirmPasswordEntry.Text))
        {
            await DisplayAlert(
                "Password",
                "Please complete all password fields.",
                "OK");

            return;
        }


        if (NewPasswordEntry.Text !=
            ConfirmPasswordEntry.Text)
        {
            await DisplayAlert(
                "Password",
                "The new passwords do not match.",
                "OK");

            return;
        }


        await DisplayAlert(
            "Password",
            "Password changes will be connected to Firebase Authentication.",
            "OK");
    }
}