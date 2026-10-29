
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Views.Parent;

public partial class ParentProfilePage : ContentPage
{
    private bool _isLoadingPreferences;

    public ParentProfilePage()
    {
        InitializeComponent();

        LoadSavedProfile();
        LoadPreferences();
    }

    private void LoadSavedProfile()
    {
        string fullName = Preferences.Get(
            "parent_full_name",
            "Parent / Guardian");

        string email = Preferences.Get(
            "parent_email",
            string.Empty);

        string phone = Preferences.Get(
            "parent_phone",
            string.Empty);

        FullNameEntry.Text = fullName;
        EmailEntry.Text = email;
        PhoneEntry.Text = phone;

        ParentNameLabel.Text = fullName;
        InitialsLabel.Text = GetInitials(fullName);
    }

    private void LoadPreferences()
    {
        _isLoadingPreferences = true;

        string savedLanguage = Preferences.Get(
            "parent_language",
            "English");

        int languageIndex = LanguagePicker.Items.IndexOf(
            savedLanguage);

        LanguagePicker.SelectedIndex = languageIndex >= 0
            ? languageIndex
            : 0;

        NotificationsSwitch.IsToggled = Preferences.Get(
            "parent_notifications_enabled",
            true);

        _isLoadingPreferences = false;
    }

    private async void OnSaveProfileClicked(
        object? sender,
        EventArgs e)
    {
        string fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string phone = PhoneEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(fullName))
        {
            await DisplayAlertAsync(
                "Missing information",
                "Please enter your full name.",
                "OK");

            return;
        }

        if (!string.IsNullOrWhiteSpace(email) &&
            !IsValidEmail(email))
        {
            await DisplayAlertAsync(
                "Invalid email",
                "Please enter a valid email address.",
                "OK");

            return;
        }

        Preferences.Set("parent_full_name", fullName);
        Preferences.Set("parent_email", email);
        Preferences.Set("parent_phone", phone);

        ParentNameLabel.Text = fullName;
        InitialsLabel.Text = GetInitials(fullName);

        await DisplayAlertAsync(
            "Profile saved",
            "Your profile details have been saved on this device. " +
            "They have not yet been synchronised with the school server.",
            "OK");
    }

    private void OnLanguageChanged(
        object? sender,
        EventArgs e)
    {
        if (_isLoadingPreferences ||
            LanguagePicker.SelectedIndex < 0)
        {
            return;
        }

        string language = LanguagePicker.SelectedItem?.ToString()
                          ?? "English";

        Preferences.Set("parent_language", language);
    }

    private void OnNotificationsToggled(
        object? sender,
        ToggledEventArgs e)
    {
        if (_isLoadingPreferences)
            return;

        Preferences.Set(
            "parent_notifications_enabled",
            e.Value);
    }

    private async void OnChangePasswordClicked(
        object? sender,
        EventArgs e)
    {
        await DisplayAlertAsync(
            "Password & Security",
            "Password changes must be handled by the authentication " +
            "service. Connect this option to your password reset " +
            "or change-password endpoint to enable it.",
            "OK");
    }

    private async void OnLogoutClicked(
        object? sender,
        EventArgs e)
    {
        bool confirm = await DisplayAlertAsync(
            "Log out",
            "Are you sure you want to log out of your account?",
            "Log out",
            "Cancel");

        if (!confirm)
            return;

        try
        {
            SecureStorage.Remove("auth_token");

            // Navigate to the login page registered in your app.
            await Shell.Current.GoToAsync("//LoginPage");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Logout issue",
                $"Unable to complete logout navigation: {ex.Message}",
                "OK");
        }
    }

    private static string GetInitials(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return "P";

        string[] parts = fullName.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
            return parts[0][0].ToString().ToUpperInvariant();

        return $"{char.ToUpperInvariant(parts[0][0])}" +
               $"{char.ToUpperInvariant(parts[^1][0])}";
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private async void OnHomeTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentDashboardPage());
    }

    private async void OnTimetableTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new TimetablePage());
    }

    private async void OnUpdatesTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new AnnouncementsPage());
    }

    private async void OnChildrenTapped(
        object? sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyChildrenPage());
    }

    private async void OnProfileTapped(
        object? sender,
        TappedEventArgs e)
    {
        // Already on the profile page.
        await Task.CompletedTask;
    }
}
