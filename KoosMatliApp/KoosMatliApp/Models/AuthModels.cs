namespace KoosMatliApp.Models;

// Information entered when a user logs in.
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

// Information entered when a parent creates an account.
public class RegisterRequest
{
    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

// Represents the user returned by the API.
public class AuthUser
{
    public string Uid { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

// Represents the response returned by the API.
public class AuthResponse
{
    public bool Success { get; set; }

    public string? Message { get; set; }

    public string? Token { get; set; }

    public AuthUser? User { get; set; }
}
