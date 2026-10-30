namespace KoosMatliApp.Features.Teacher.Models;

public class FirebaseLoginResult
{
    public bool Success { get; set; }


    public string Message { get; set; } = string.Empty;


    public string Role { get; set; } = string.Empty;


    public string UserId { get; set; } = string.Empty;


    public string DisplayName { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;
}
