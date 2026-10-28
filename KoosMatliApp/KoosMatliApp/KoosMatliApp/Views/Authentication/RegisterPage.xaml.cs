
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Authentication;

public partial class RegisterPage : ContentPage
{
    private bool _isRegistering;
    private bool _isPasswordVisible;

    public RegisterPage()
    {
        InitializeComponent();
    }

    private void OnTogglePasswordVisibility(object sender, EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        PasswordEntry.IsPassword = !_isPasswordVisible;
        ConfirmPasswordEntry.IsPassword = !_isPasswordVisible;

        PasswordVisibilityButton.Text =
            _isPasswordVisible ? "Hide" : "Show";
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (_isRegistering)
            return;

        string displayName = DisplayNameEntry.Text?.Trim() ?? string.Empty;
        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;
        string confirmPassword = ConfirmPasswordEntry.Text ?? string.Empty;

        StatusLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(displayName))
        {
            ShowMessage("Please enter your full name.");
            return;
        }

        if (string.IsNullOrWhiteSpace(email) ||
            !email.Contains('@') ||
            !email.Contains('.'))
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        if (password.Length < 8)
        {
            ShowMessage("Your password must contain at least 8 characters.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowMessage("Your passwords do not match.");
            return;
        }

        try
        {
            _isRegistering = true;
            RegisterButton.IsEnabled = false;
            RegisterButton.Text = "Creating account...";

            RegisterActivityIndicator.IsVisible = true;
            RegisterActivityIndicator.IsRunning = true;

            // Calls your existing backend registration service.
            AuthService.Instance.Logout();

            var response = await AuthService.Instance.RegisterParentAsync(
                displayName,
                email,
                password);

            if (!response.Success)
            {
                ShowMessage(
                    response.Message ??
                    "Unable to create your account. Please try again.");

                return;
            }

            await DisplayAlertAsync(
                "Registration Successful",
                "Your parent account has been created. You can now sign in.",
                "Continue");

            // Return to the login page.
            await Navigation.PopAsync();
        }
        catch (Exception)
        {
            ShowMessage(
                "Something went wrong during registration. Please try again.");
        }
        finally
        {
            _isRegistering = false;
            RegisterButton.IsEnabled = true;
            RegisterButton.Text = "Create Account";

            RegisterActivityIndicator.IsVisible = false;
            RegisterActivityIndicator.IsRunning = false;
        }
    }

    private async void OnSignInTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ShowMessage(string message)
    {
        StatusLabel.Text = message;
        StatusLabel.IsVisible = true;
    }
}
