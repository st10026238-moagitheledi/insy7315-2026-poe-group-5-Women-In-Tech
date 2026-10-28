using System;
using Microsoft.Maui.Controls;
using KoosMatliApp.Services;

namespace KoosMatliApp.Pages;

public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();
	}

	private async void SendCode_Clicked(object sender, EventArgs e)
	{
		// Source: AuthService token/code generation pattern (AuthService.GenerateResetCode) [4] (2026)
		var email = ForgotEmailEntry.Text?.Trim();
		if (string.IsNullOrEmpty(email))
		{
			await DisplayAlert("Validation", "Please enter an email.", "OK");
			return;
		}

		var user = await AuthService.FindAsync(email);
		if (user == null)
		{
			await DisplayAlert("Not Found", "No account with that email.", "OK");
			return;
		}

		var code = AuthService.GenerateResetCode(email);
		// In a real app, send via email/SMS. Here show alert for demo.
		await DisplayAlert("Code Sent", $"Verification code: {code}", "OK");
		await Navigation.PushAsync(new VerifyPage(email));
	}

	private async void BackToLogin_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}

// References:
// [1] Microsoft Learn - .NET MAUI Preferences - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/preferences [Accessed 2026]
// [4] Auth patterns (generate/verify reset codes) - internal design; see AuthService.cs [Accessed 2026]
