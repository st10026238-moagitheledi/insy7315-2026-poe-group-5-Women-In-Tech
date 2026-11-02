using System.Net.Mail; 
using KoosMatliApp.Models;
using KoosMatliApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KoosMatliApp.Views.Admin;

public partial class AddNewUserPage : ContentPage
{
    private readonly IAdminApiService _api;

    public AddNewUserPage()
    {
        InitializeComponent();
        RolePicker.ItemDisplayBinding = new Binding(nameof(RoleChoice.Display));
        RolePicker.ItemsSource = new[] { "Teacher", "Parent", "Learner" }.Select(value => new RoleChoice(value, L.T(value))).ToList();
        _api = App.Services!.GetRequiredService<IAdminApiService>(); //(Microsoft Learn, 2026c).
    }

    private async void SaveButton_Clicked(object sender, EventArgs e) //(Microsoft Learn, 2026c).
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            RolePicker.SelectedItem == null)
        {
            ErrorLabel.Text = L.T("Please complete the name, surname, role and email fields.");
            ErrorLabel.IsVisible = true;
            return;
        }

        try
        {
            _ = new MailAddress(EmailEntry.Text.Trim()); //(Microsoft Learn, 2026a).
        }
        catch
        {
            ErrorLabel.Text = L.T("Please enter a valid email address.");
            ErrorLabel.IsVisible = true;
            return;
        }

        SaveButton.IsEnabled = false;

        try
        {
            var user = new User
            {
                FirstName = FirstNameEntry.Text.Trim(),
                LastName = LastNameEntry.Text.Trim(),
                Role = ((RoleChoice)RolePicker.SelectedItem).Value, //(Microsoft Learn, 2026b).
                Email = EmailEntry.Text.Trim(),
                PhoneNumber = PhoneEntry.Text?.Trim() ?? string.Empty
            };

            var savedUser = await _api.AddUserAsync(user);

            if (savedUser != null)
            {
                await DisplayAlertAsync(
                    L.T("Success"),
                    L.F("{0} was added successfully.", savedUser.FullName),
                    L.T("OK"));

                await Shell.Current.GoToAsync(".."); //(Microsoft Learn, 2026d).
            }
            else
            {
                ErrorLabel.Text = L.T("The API did not return the saved user.");
                ErrorLabel.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}

public sealed record RoleChoice(string Value, string Display);

/*Reference List
 
Microsoft Corporation, "MailAddress Class (System.Net.Mail)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress

Microsoft Corporation, "Picker Control and ItemDisplayBinding in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/picker

Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

Microsoft Corporation, "Shell Navigation in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation
 */