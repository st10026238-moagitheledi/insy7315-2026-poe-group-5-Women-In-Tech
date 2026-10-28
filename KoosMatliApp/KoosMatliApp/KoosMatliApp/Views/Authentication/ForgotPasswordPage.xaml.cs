using KoosMatliApp.Models;
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Authentication;

public partial class ForgotPasswordPage : ContentPage
{
    private bool _isSending;

    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    private async void OnResetClicked(object sender, EventArgs e)
    {
        if (_isSending)
            return;

        string email = EmailEntry.Text?.Trim() ?? string.Empty;

        StatusLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(email) ||
            !email.Contains('@') ||
            !email.Contains('.'))
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        try
        {
            _isSending = true;

            ResetButton.IsEnabled = false;
            ResetButton.Text = "Sending...";

            ResetActivityIndicator.IsVisible = true;
            ResetActivityIndicator.IsRunning = true;

            // Send the email address to the Node.js API.
            AuthResponse response =
                await AuthService.Instance.ForgotPasswordAsync(email);

            if (!response.Success)
            {
                ShowMessage(
                    response.Message ??
                    "Unable to process your request. Please try again.");

                return;
            }

            await DisplayAlertAsync(
                "Check Your Email",
                response.Message ??
                "If an account exists for that email, password reset instructions will be sent.",
                "OK");
        }
        catch (Exception)
        {
            ShowMessage(
                "Something went wrong. Please try again.");
        }
        finally
        {
            _isSending = false;

            ResetButton.IsEnabled = true;
            ResetButton.Text = "Send Reset Instructions";

            ResetActivityIndicator.IsVisible = false;
            ResetActivityIndicator.IsRunning = false;
        }
    }

    private async void OnBackToLoginClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void ShowMessage(string message)
    {
        StatusLabel.Text = message;
        StatusLabel.IsVisible = true;
    }
}
