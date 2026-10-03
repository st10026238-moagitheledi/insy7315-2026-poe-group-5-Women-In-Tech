using KoosMatliAdminApi.Models;

namespace KoosMatliAdminApi.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Users.Any() || context.Applications.Any())
            return; // already seeded

        // Users
        var users = new List<User>
        {
            new User { FirstName = "Alice", LastName = "Miller", Email = "alice@school.edu", PhoneNumber = "0711000001", Role = "Parent" },
            new User { FirstName = "Bob", LastName = "Smith", Email = "bob@school.edu", PhoneNumber = "0711000002", Role = "Teacher" },
            new User { FirstName = "Charlie", LastName = "Brown", Email = "charlie@school.edu", PhoneNumber = "0711000003", Role = "Learner" }
        };

        context.Users.AddRange(users);

        // Applications
        var apps = new List<AdmissionApplication>
        {
            new AdmissionApplication
            {
                ApplicationNumber = "APP-2026-001",
                LearnerFirstName = "Daisy",
                LearnerLastName = "Jones",
                DateOfBirth = DateTime.Now.AddYears(-8),
                GradeApplyingFor = "Grade 3",
                GuardianName = "Eve Jones",
                GuardianEmail = "eve.jones@example.com",
                GuardianPhone = "0711222333",
                DateSubmitted = DateTime.UtcNow.AddDays(-3),
                Status = "Pending"
            },
            new AdmissionApplication
            {
                ApplicationNumber = "APP-2026-002",
                LearnerFirstName = "Frank",
                LearnerLastName = "Green",
                DateOfBirth = DateTime.Now.AddYears(-10),
                GradeApplyingFor = "Grade 5",
                GuardianName = "Grace Green",
                GuardianEmail = "grace.green@example.com",
                GuardianPhone = "0711444555",
                DateSubmitted = DateTime.UtcNow.AddDays(-1),
                Status = "New"
            }
        };

        context.Applications.AddRange(apps);

        // Staff
        var staff = new List<StaffMember>
        {
            new StaffMember { FirstName = "Bob", LastName = "Smith", Email = "bob@school.edu", Position = "Teacher", Subjects = new List<string> { "Math" }, Classes = new List<string>{ "Grade 3A" } },
            new StaffMember { FirstName = "Helen", LastName = "Clark", Email = "helen@school.edu", Position = "Principal" }
        };

        context.StaffMembers.AddRange(staff);

        // School details
        var details = new SchoolDetails
        {
            SchoolName = "Koos Matli Primary",
            Address = "123 School Lane",
            PhoneNumber = "0711999888",
            Email = "info@koosmatli.edu",
            PrincipalName = "Helen Clark"
        };

        context.SchoolDetails.Add(details);

        // School dates
        var dates = new List<SchoolDate>
        {
            new SchoolDate { Title = "First Term Start", Date = DateTime.UtcNow.Date.AddDays(7), Description = "First term begins" },
            new SchoolDate { Title = "Midterm Break", Date = DateTime.UtcNow.Date.AddDays(45), Description = "Midterm" }
        };

        context.SchoolDates.AddRange(dates);

        // Calendar events
        var events = new List<CalendarEvent>
        {
            new CalendarEvent { Title = "Sports Day", Start = DateTime.UtcNow.Date.AddDays(30), End = DateTime.UtcNow.Date.AddDays(30), Description = "Annual sports day" }
        };

        context.Set<CalendarEvent>().AddRange(events);

        // Documents
        var docs = new List<SchoolDocument>
        {
            new SchoolDocument { Title = "School Prospectus", FileName = "prospectus.pdf", FileUrl = "/files/prospectus.pdf", UploadedDate = DateTime.UtcNow.AddDays(-10), Category = "General" }
        };

        context.SchoolDocuments.AddRange(docs);

        context.SaveChanges();
    }
}
