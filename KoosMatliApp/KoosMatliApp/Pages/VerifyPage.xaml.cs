using Microsoft.Maui.Controls;
using KoosMatliApp.Services;

namespace KoosMatliApp.Pages;

// No-op patch: file update only.
public partial class VerifyPage : ContentPage
{
    private readonly string _email;

    public VerifyPage(string email)
    {
        InitializeComponent();
        _email = email;

        // Automatically shift focus to the next field as the user types
        D0.TextChanged += (s, e) => { if (e.NewTextValue?.Length == 1) D1.Focus(); };
        D1.TextChanged += (s, e) => { if (e.NewTextValue?.Length == 1) D2.Focus(); };
        D2.TextChanged += (s, e) => { if (e.NewTextValue?.Length == 1) D3.Focus(); };
        D3.TextChanged += (s, e) => { if (e.NewTextValue?.Length == 1) D4.Focus(); };
        D4.TextChanged += (s, e) => { if (e.NewTextValue?.Length == 1) D5.Focus(); };
    }

    private async void Verify_Clicked(object sender, EventArgs e)
    {
        // Source: AuthService verify pattern (in-memory reset code store) [4] (2026)
        var code = string.Concat(
            D0.Text ?? "",
            D1.Text ?? "",
            D2.Text ?? "", 
            D3.Text ?? "", 
            D4.Text ?? "", 
            D5.Text ?? ""
            ).Trim();
        if (string.IsNullOrWhiteSpace(code) || code.Length != 6)
        {
            await DisplayAlert("Validation", "Enter the 6-digit code.", "OK");
            return;
        }

        if (!AuthService.VerifyResetCode(_email, code))
        {
            await DisplayAlert("Invalid", "Code incorrect.", "OK");
            return;
        }

        await Navigation.PushAsync(new ResetPassword(_email));
    }

    private async void Resend_Clicked(object sender, EventArgs e)
    {
        // Source: AuthService.GenerateResetCode usage - demonstration of resend pattern [4] (2026)
        var code = AuthService.GenerateResetCode(_email);
        await DisplayAlert("Resent", $"New code: {code}", "OK");
    }
}

// References:
// [4] Auth patterns (generate/verify reset codes) - see AuthService.cs implementation [Accessed 2026]
// [2] Microsoft Learn - C# async/await - https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/async/ [Accessed 2026]
