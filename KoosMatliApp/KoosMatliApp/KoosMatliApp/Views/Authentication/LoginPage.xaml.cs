using KoosMatliApp.Services;
using KoosMatliApp.Views.Parent;
using Microsoft.Maui.Controls;

namespace KoosMatliApp.Views.Authentication;

public partial class LoginPage : ContentPage
{
    private bool _isLoggingIn;
    private bool _isPasswordVisible;

    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnTogglePasswordVisibility(
        object sender,
        EventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        PasswordEntry.IsPassword = !_isPasswordVisible;

        PasswordVisibilityButton.Text =
            _isPasswordVisible ? "Hide" : "Show";
    }

    private async void OnLoginClicked(
        object sender,
        EventArgs e)
    {
        if (_isLoggingIn)
            return;

        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text ?? string.Empty;

        StatusLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ShowMessage(
                "Please enter both your email address and password.");
            return;
        }

        if (!email.Contains('@') || !email.Contains('.'))
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        try
        {
            _isLoggingIn = true;

            LoginButton.IsEnabled = false;
            LoginButton.Text = "Signing in...";

            LoginActivityIndicator.IsVisible = true;
            LoginActivityIndicator.IsRunning = true;

            var response = await AuthService.Instance.LoginAsync(
                email,
                password);

            if (!response.Success || response.User == null)
            {
                ShowMessage(
                    response.Message ??
                    "Unable to sign in. Please check your details.");
                return;
            }

            if (!response.User.IsActive)
            {
                ShowMessage(
                    "Your account is inactive. Please contact the school.");
                return;
            }

            string role = response.User.Role
                .Trim()
                .ToLowerInvariant();

            switch (role)
            {
                case "parent":
                    // Open the parent dashboard.
                    await Navigation.PushAsync(
                        new ParentDashboardPage());

                    break;

                case "teacher":
                    await DisplayAlertAsync(
                        "Teacher Login",
                        "You have signed in successfully. The teacher landing page has not been connected yet.",
                        "OK");

                    break;

                case "admin":
                case "administrator":
                    await DisplayAlertAsync(
                        "Administrator Login",
                        "You have signed in successfully. The administrator landing page has not been connected yet.",
                        "OK");

                    break;

                default:
                    ShowMessage(
                        "Your account role is not recognised. Please contact the school.");
                    break;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Login error: {ex}");

            ShowMessage(
                "Something went wrong while signing in. Please try again.");
        }
        finally
        {
            _isLoggingIn = false;

            LoginButton.IsEnabled = true;
            LoginButton.Text = "Sign In";

            LoginActivityIndicator.IsVisible = false;
            LoginActivityIndicator.IsRunning = false;
        }
    }

    private async void OnForgotPasswordTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }

    private async void OnRegisterTapped(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private void ShowMessage(string message)
    {
        StatusLabel.Text = message;
        StatusLabel.IsVisible = true;
    }
}
