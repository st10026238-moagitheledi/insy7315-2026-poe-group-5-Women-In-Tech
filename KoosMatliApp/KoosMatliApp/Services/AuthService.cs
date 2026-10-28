using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoosMatliApp.Models;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Services;
public static class AuthService
{
    static Dictionary<string, string> _resetCodes = new();

    public static async Task<bool> RegisterAsync(User u)
    {
        await UserRepository.InitializeAsync();
        var existing = await UserRepository.GetByEmailAsync(u.Email);
        if (existing != null) return false;
        var entity = new UserEntity { Email = u.Email, Password = u.Password, FirstName = u.FirstName, LastName = u.LastName, Phone = u.Phone, Role = u.Role };
        await UserRepository.AddUserAsync(entity);
        return true;
    }
    // Source: sqlite-net examples for async DB access and inserts [3] (2026)
    public static async Task<bool> ValidateAsync(string email, string password)
    {
        await UserRepository.InitializeAsync();
        return await UserRepository.ValidateAsync(email, password);
    }

    public static async Task<User?> FindAsync(string email)
    {
        await UserRepository.InitializeAsync();
        var e = await UserRepository.GetByEmailAsync(email);
        if (e == null) return null;
        return new User(e.Email, e.Password) { FirstName = e.FirstName, LastName = e.LastName, Phone = e.Phone };
    }

    public static string GenerateResetCode(string email)
    {
        var code = new System.Random().Next(100000, 999999).ToString();
        _resetCodes[email] = code;
        return code;
    }
 public static bool VerifyResetCode(string email, string code)
    {
        return _resetCodes.TryGetValue(email, out var stored) && stored == code;
    }

    public static async Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        await UserRepository.InitializeAsync();
        var ok = await UserRepository.UpdatePasswordAsync(email, newPassword);
        if (ok) _resetCodes.Remove(email);
        return ok;
    }
}

// References:
// [1] Microsoft Learn - .NET MAUI Preferences - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/preferences [Accessed 2026]
// [2] Microsoft Learn - C# async/await - https://learn.microsoft.com/dotnet/csharp/programming-guide/concepts/async/ [Accessed 2026]
// [3] sqlite-net-pcl (NuGet) - package and usage: https://www.nuget.org/packages/sqlite-net-pcl [Accessed 2026]
// [4] Microsoft Learn - Local databases in .NET MAUI - https://learn.microsoft.com/dotnet/maui/data-cloud/local-databases [Accessed 2026]
// [5] Microsoft Learn - .NET MAUI Navigation - https://learn.microsoft.com/dotnet/maui/fundamentals/navigation [Accessed 2026]

