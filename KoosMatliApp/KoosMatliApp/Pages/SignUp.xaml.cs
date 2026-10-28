using KoosMatliApp.Models;
using KoosMatliApp.Resources.Localization;
using KoosMatliApp.Services;
using Microsoft.Maui.Controls;
using System;

namespace KoosMatliApp.Pages;

public partial class SignUp : ContentPage
{
	public SignUp()
	{
		InitializeComponent();
	}

    private async void SignUpTogglePwdBtn_Clicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        SignUpTogglePwdBtn.Text = PasswordEntry.IsPassword ?
            AppResources.TxtShow :
            AppResources.TxtHide;
    }

    private void ToggleConfirm_Clicked(object sender, EventArgs e)
	{
		ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
		ToggleConfirmBtn.Text = ConfirmPasswordEntry.IsPassword ?
			AppResources.TxtShow:
            AppResources.TxtHide;
	}

	private async void SignUp_Clicked(object sender, EventArgs e)
	{
        // Microsoft Learn (2026)
        var fn = FirstNameEntry.Text?.Trim();
		var ln = LastNameEntry.Text?.Trim();
		var email = EmailEntry.Text?.Trim();
		var phone = PhoneEntry.Text?.Trim();
		var pw = PasswordEntry.Text;
		var cpw = ConfirmPasswordEntry.Text;
		var role = RolePicker.SelectedItem?.ToString();

        if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pw))
		{
			await DisplayAlert("Validation", "Please complete required fields.", "OK");
			return;
		}

		if (string.IsNullOrWhiteSpace(role) || (role != "Parent" && role != "Teacher"))
        {
            await DisplayAlert("Validation", "Please select a role.", "OK");
            return;
        }

        if (pw != cpw)
		{
			await DisplayAlert("Validation", "Passwords do not match.", "OK");
			return;
		}

		if (!AgreeCheckbox.IsChecked)
		{
			await DisplayAlert("Validation", "You must agree to the Terms.", "OK");
			return;
		}

		var user = new User(email, pw!)
		{
			FirstName = fn,
			LastName = ln,
			Phone = phone,
			Role = role
		};

		var ok = await AuthService.RegisterAsync(user);
		if (!ok)
		{
			await DisplayAlert("Sign Up", "An account with this email already exists.", "OK");
			return;
		}

		await DisplayAlert("Success", $"Your {role.ToLowerInvariant()} account created. Please log in.", "OK");
		await Navigation.PushAsync(new Login());
	}

	private async void GoToLogin_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new Login());
	}
}

// References:
// Microsoft Learn - C# async/await - https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/async/ [Accessed 2026]
// sqlite-net-pcl (NuGet) - package and usage: https://www.nuget.org/packages/sqlite-net-pcl [Accessed 2026]
// Microsoft Learn - Local databases in .NET MAUI - https://learn.microsoft.com/dotnet/maui/data-cloud/local-databases [Accessed 2026]
