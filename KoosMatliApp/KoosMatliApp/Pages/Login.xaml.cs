using KoosMatliApp.Resources.Localization;
using KoosMatliApp.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;

namespace KoosMatliApp.Pages;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	private async void LoginTogglePwdBtn_Clicked(object sender, EventArgs e)
	{
        LoginPasswordEntry.IsPassword = !LoginPasswordEntry.IsPassword;

        LoginTogglePwdBtn.Text = LoginPasswordEntry.IsPassword ?
            AppResources.TxtShow :
            AppResources.TxtHide;
    }

	private async void Login_Clicked(object sender, EventArgs e)
	{
		// Microsoft Learn (2026)
		var email = EmailOrUsernameEntry.Text?.Trim();
		var pw = LoginPasswordEntry.Text;
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pw))
		{
			await DisplayAlert("Login", "Please enter email and password.", "OK");
			return;
		}

		if (!await AuthService.ValidateAsync(email, pw))
		{
			await DisplayAlert("Login Failed", "Invalid credentials.", "OK");
			return;
		}

		// AuthService exposes ValidateAsync; use await to call it correctly
		if (RememberMeCheckbox.IsChecked)
		{
			Preferences.Set("remembered_email", email);
		}
		//parent and/or teacher dashboard
		//await Navigation.PushAsync(new Staff());
	}

	private async void GoToSignUp_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new SignUp());
	}

	private async void ForgotPassword_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new ForgotPassword());
	}
}

// References:
// Microsoft Learn - .NET MAUI Preferences - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/preferences [Accessed 2026]
// Microsoft Learn - C# async/await - https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/async/ [Accessed 2026]
