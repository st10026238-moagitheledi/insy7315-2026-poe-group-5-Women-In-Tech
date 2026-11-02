using KoosMatliApp.Services;
using KoosMatliApp.Views.Parent;
using KoosMatliApp.Features.Teacher.Views;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

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
                    await DisplayAlertAsync(
                        "Welcome",
                        $"Welcome back, {response.User.DisplayName}!",
                        "Continue");

                    // Open the parent dashboard.
                    await Navigation.PushAsync(
                        new ParentDashboardPage());

                    break;

                case "teacher":
                    await Navigation.PushAsync(
                       new TeacherHomePage());

                    break;

                case "admin":
                case "administrator":
                    {
                        var services = App.Services
                            ?? throw new InvalidOperationException(
                                "Application services are unavailable.");

                        var session =
                            services.GetRequiredService<AdminSession>();

                        var settings =
                            services.GetRequiredService<SettingsService>();

                        var api =
                            services.GetRequiredService<IAdminApiService>();

                        var token =
                            await AuthService.Instance.GetSavedTokenAsync();

                        if (string.IsNullOrWhiteSpace(token))
                        {
                            ShowMessage(
                                "Login succeeded, but no saved token was found.");
                            return;
                        }

                        session.Connect(
                            response.User.Uid,
                            response.User.Email,
                            _ => AuthService.Instance.GetSavedTokenAsync());

                        settings.Reset();

                        try
                        {
                            var dashboard = await api.GetDashboardAsync();

                            if (dashboard is null)
                            {
                                throw new InvalidOperationException(
                                    "The API returned no administrator dashboard.");
                            }

                            await settings.LoadAsync();

                            if (!session.IsConnected)
                            {
                                throw new InvalidOperationException(
                                    "The administrator session was cleared during startup.");
                            }
                        }
                        catch (Exception ex)
                        {
                            session.Clear();
                            settings.Reset();

                            ShowMessage(ex.Message);
                            return;
                        }

                        App.ShowShell();
                        break;
                    }

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
