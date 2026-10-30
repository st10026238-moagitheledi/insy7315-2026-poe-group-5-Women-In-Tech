#if DEBUG
using System.Net.Http.Json;
using System.Text.Json;

namespace KoosMatliApp.Views;

public sealed class AdminTestSignInPage : ContentPage
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    private readonly Entry _key = new()
    {
        Placeholder = "Firebase Web API key"
    };

    private readonly Entry _email = new()
    {
        Placeholder = "Email",
        Keyboard = Keyboard.Email,
        Text = "test@email.com"
    };

    private readonly Entry _password = new()
    {
        Placeholder = "Password",
        IsPassword = true
    };

    private readonly Label _message = new()
    {
        TextColor = Colors.Red
    };

    private readonly Button _button = new()
    {
        Text = "Sign in and test admin"
    };

    private readonly ActivityIndicator _busy = new();

    public AdminTestSignInPage()
    {
        Title = "Admin test sign-in";

        _button.Clicked += SignInClicked;

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = 24,
                Spacing = 16,
                MaximumWidthRequest = 600,
                Children =
                {
                    new Label
                    {
                        Text = "Admin testing",
                        FontSize = 28,
                        FontAttributes = FontAttributes.Bold
                    },
                    new Label
                    {
                        Text =
                            "Temporary Debug-only sign-in. " +
                            "Credentials are not saved."
                    },
                    _key,
                    _email,
                    _password,
                    _message,
                    _busy,
                    _button
                }
            }
        };
    }

    private async void SignInClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_key.Text) ||
            string.IsNullOrWhiteSpace(_email.Text) ||
            string.IsNullOrEmpty(_password.Text))
        {
            _message.Text =
                "Enter the Web API key, test email and password.";
            return;
        }

        _button.IsEnabled = false;
        _key.IsEnabled = false;
        _email.IsEnabled = false;
        _password.IsEnabled = false;

        _busy.IsRunning = true;
        _message.Text = "";

        try
        {
            var result = await SignInForTestAsync(
                Client,
                _key.Text.Trim(),
                _email.Text.Trim(),
                _password.Text);

            _password.Text = "";

            await App.EnterAdminAsync(
                result.Uid,
                result.Email,
                _ => Task.FromResult<string?>(result.Token));
        }
        catch (Exception ex)
        {
            _message.Text = ex.Message;
        }
        finally
        {
            _password.Text = "";

            _button.IsEnabled = true;
            _key.IsEnabled = true;
            _email.IsEnabled = true;
            _password.IsEnabled = true;

            _busy.IsRunning = false;
        }
    }

    internal static async Task<(
        string Uid,
        string Email,
        string Token)> SignInForTestAsync(
            HttpClient client,
            string webApiKey,
            string email,
            string password)
    {
        using var response = await client.PostAsJsonAsync(
            "https://identitytoolkit.googleapis.com/v1/" +
            "accounts:signInWithPassword?key=" +
            Uri.EscapeDataString(webApiKey),
            new
            {
                email,
                password,
                returnSecureToken = true
            });

        using var document = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        var root = document.RootElement;

        if (!response.IsSuccessStatusCode)
        {
            var code =
                root.TryGetProperty("error", out var error) &&
                error.TryGetProperty("message", out var message)
                    ? message.GetString()
                    : "Firebase sign-in failed.";

            throw new InvalidOperationException(code switch
            {
                "INVALID_LOGIN_CREDENTIALS" or
                "INVALID_PASSWORD" or
                "EMAIL_NOT_FOUND" =>
                    "The email or password is incorrect.",

                "OPERATION_NOT_ALLOWED" =>
                    "Enable Email/Password in Firebase Authentication.",

                "USER_DISABLED" =>
                    "The test account is disabled.",

                _ => code ?? "Firebase sign-in failed."
            });
        }

        var uid = root.GetProperty("localId").GetString();
        var token = root.GetProperty("idToken").GetString();

        if (string.IsNullOrWhiteSpace(uid) ||
            string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                "Firebase did not return a user and token.");
        }

        return (
            uid,
            root.GetProperty("email").GetString() ?? email,
            token);
    }
}
#endif