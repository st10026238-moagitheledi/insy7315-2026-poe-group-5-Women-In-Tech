using System;
using System.Collections.Generic;
using System.Text;

namespace KoosMatliApp.Features.Teacher.Models
{
    public class Teacher
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty; // (Microsoft, 2026)

        public string TeacherNumber { get; set; } = string.Empty;
    }

}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

