using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace KoosMatliApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; } = "Parent";

        public User()
        {
        }

        public User(string firstname, string lastname, string email, string password, string phone, string role)
        {
            Email = email;
            Password = password;
            Role = role;
            FirstName = firstname;
            LastName = lastname;
            Phone = phone;
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

//References:

//Microsoft Learn - Local databases in .NET MAUI - https://learn.microsoft.com/dotnet/maui/data-cloud/local-databases [Accessed 2026]
// sqlite-net-pcl (NuGet) - package and usage: https://www.nuget.org/packages/sqlite-net-pcl [Accessed 2026]
