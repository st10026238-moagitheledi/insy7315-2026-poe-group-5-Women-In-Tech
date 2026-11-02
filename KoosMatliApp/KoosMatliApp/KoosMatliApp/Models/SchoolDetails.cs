namespace KoosMatliApp.Models;

public class SchoolDetails
{
    public int Id { get; set; }

    public string SchoolName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty; //(Microsoft Learn, 2026).

    public string PrincipalName { get; set; } = string.Empty;
}
/*Reference List
 
 Microsoft Corporation, "Model-View-ViewModel (MVVM) in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/mvvm
 */