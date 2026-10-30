namespace KoosMatliApp.Models;

public class User
{
    public int Id { get; set; } //(Microsoft Learn, 2026).

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */