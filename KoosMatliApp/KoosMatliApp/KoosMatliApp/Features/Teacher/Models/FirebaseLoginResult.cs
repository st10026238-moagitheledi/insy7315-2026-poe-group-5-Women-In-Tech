namespace KoosMatliApp.Features.Teacher.Models;

public class FirebaseLoginResult
{
    public bool Success { get; set; }


    public string Message { get; set; } = string.Empty;


    public string Role { get; set; } = string.Empty; // (Microsoft, 2026)


    public string UserId { get; set; } = string.Empty;


    public string DisplayName { get; set; } = string.Empty; // (Microsoft, 2026)


    public string Email { get; set; } = string.Empty;
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
