using System;
using Microsoft.Maui.Controls;
using KoosMatliApp.Services;

namespace KoosMatliApp.Pages;

public partial class ResetPassword : ContentPage
{
	private readonly string _email;

	public ResetPassword(string email)
	{
		InitializeComponent();
		_email = email;
	}

    private void ToggleNewPassword_Clicked(object sender, EventArgs e)
    {
        NewPasswordEntry.IsPassword = !NewPasswordEntry.IsPassword;
        ToggleNewPasswordButton.Source = NewPasswordEntry.IsPassword ? "ic_eye_hide.png" : "ic_eye_show.png";
    }

    private void ToggleConfirmPassword_Clicked(object sender, EventArgs e)
    {
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        ToggleConfirmPasswordButton.Source = ConfirmPasswordEntry.IsPassword ? "ic_eye_hide.png" : "ic_eye_show.png";
    }

    private async void Reset_Clicked(object sender, EventArgs e)
	{
        // Auth patterns (2026)
		var np = NewPasswordEntry.Text;
		var cp = ConfirmPasswordEntry.Text;

		if (string.IsNullOrEmpty(np) || string.IsNullOrEmpty(cp))
		{
			await DisplayAlert("Validation", "Please enter the code and new password.", "OK");
			return;
		}

		if (np.Length < 8 || np.Length > 60)
		{
			await DisplayAlert("Validation", "Password must be between 8 and 60 characters.", "OK");
			return;
        }

		if (np != cp)
		{
			await DisplayAlert("Validation", "Passwords do not match.", "OK");
			return;
		}

		var ok = await AuthService.ResetPasswordAsync(_email, np);
		if (!ok)
		{
			await DisplayAlert("Error", "Could not reset password.", "OK");
			return;
		}

		await DisplayAlert("Success", "Password updated. Please log in.", "OK");


        Application.Current.MainPage = new NavigationPage(new Login());
    }
}

// References:
// Auth patterns (generate/verify reset codes and reset password) - see AuthService.cs [Accessed 2026]
// Microsoft Learn - C# async/await - https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/async/ [Accessed 2026]

