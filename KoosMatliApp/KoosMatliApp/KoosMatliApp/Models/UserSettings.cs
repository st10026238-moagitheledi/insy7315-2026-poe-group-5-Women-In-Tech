namespace KoosMatliApp.Models;
public sealed class UserSettings
{
    public string Language { get; set; } = "en-ZA";
    public string Theme { get; set; } = "System";
    public bool NotificationsEnabled { get; set; } = true;
    public string DisplayName { get; set; } = ""; //(Microsoft Learn, 2026).
    public string PhoneNumber { get; set; } = "";
    public bool ConfirmSensitiveActions { get; set; } = true;
}

/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */
