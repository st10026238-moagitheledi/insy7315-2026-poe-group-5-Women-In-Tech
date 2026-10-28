using System.Threading.Tasks;
using SQLite;
using System.IO;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Services;

public class UserEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
}

public static class UserRepository
{
    static SQLiteAsyncConnection? _db;

    public static async Task InitializeAsync()
    {
        if (_db != null) return;
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db3");
        _db = new SQLiteAsyncConnection(dbPath);
        await _db.CreateTableAsync<UserEntity>();
    }

    public static Task<int> AddUserAsync(UserEntity u)
    {
        if (_db == null) throw new InvalidOperationException("DB not initialized");
        return _db.InsertAsync(u);
    }

    public static async Task<UserEntity?> GetByEmailAsync(string email)
    {
        if (_db == null) throw new InvalidOperationException("DB not initialized");
        var list = await _db.Table<UserEntity>().Where(x => x.Email == email).ToListAsync();
        return list.FirstOrDefault();
    }

    public static async Task<bool> ValidateAsync(string email, string password)
    {
        var u = await GetByEmailAsync(email);
        return u != null && u.Password == password;
    }

    public static async Task<bool> UpdatePasswordAsync(string email, string newPassword)
    {
        var u = await GetByEmailAsync(email);
        if (u == null) return false;
        u.Password = newPassword;
        await _db!.UpdateAsync(u);
        return true;
    }
}

// References:
// sqlite-net-pcl (NuGet) - package and usage: https://www.nuget.org/packages/sqlite-net-pcl [Accessed 2026]
// Microsoft Learn - Local databases in .NET MAUI - https://learn.microsoft.com/dotnet/maui/data-cloud/local-databases [Accessed 2026]
// Microsoft Learn - FileSystem.AppDataDirectory - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/file-system [Accessed 2026]
