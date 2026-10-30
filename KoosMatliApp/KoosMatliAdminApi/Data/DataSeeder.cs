using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.Services;

namespace KoosMatliAdminApi.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(FirestoreDataStore store)
    {
        var today = DateTime.UtcNow.Date;

        await AddMissing(store, "applications", new[]
        {
            Application(
                "DEMO-APP-001", "Demo Naledi", "Mokoena",
                "Grade 1", "New", 6, today),

            Application(
                "DEMO-APP-002", "Demo Kabelo", "Dube",
                "Grade 3", "Pending", 8, today),

            Application(
                "DEMO-APP-003", "Demo Lerato", "Molefe",
                "Grade 5", "Approved", 10, today),

            Application(
                "DEMO-APP-004", "Demo Thabo", "Nkosi",
                "Grade 2", "Rejected", 7, today)
        }, x => x.ApplicationNumber);

        await AddMissing(store, "staffMembers", new[]
        {
            new StaffMember
            {
                FirstName = "Demo Dineo",
                LastName = "Molefe",
                Email = "demo.teacher1@example.com",
                Position = "Teacher",
                Subjects = new()
                {
                    "[DEMO] Mathematics",
                    "[DEMO] English"
                },
                Classes = new()
                {
                    "[DEMO] Grade 3A"
                }
            },
            new StaffMember
            {
                FirstName = "Demo Thabo",
                LastName = "Mokoena",
                Email = "demo.teacher2@example.com",
                Position = "Teacher",
                Subjects = new()
                {
                    "[DEMO] Natural Sciences"
                },
                Classes = new()
                {
                    "[DEMO] Grade 5A"
                }
            }
        }, x => x.Email);

        await AddMissing(store, "schoolClasses", new[]
        {
            new SchoolClass
            {
                ClassName = "[DEMO] Grade 3A",
                Grade = "Grade 3",
                TeacherName = "Demo Dineo Molefe",
                LearnerCount = 0
            },
            new SchoolClass
            {
                ClassName = "[DEMO] Grade 5A",
                Grade = "Grade 5",
                TeacherName = "Demo Thabo Mokoena",
                LearnerCount = 0
            }
        }, x => x.ClassName);

        await AddMissing(store, "subjects", new[]
        {
            new Subject
            {
                Name = "[DEMO] Mathematics",
                Grade = "Grade 3",
                TeacherNames = new() { "Demo Dineo Molefe" }
            },
            new Subject
            {
                Name = "[DEMO] English",
                Grade = "Grade 3",
                TeacherNames = new() { "Demo Dineo Molefe" }
            },
            new Subject
            {
                Name = "[DEMO] Natural Sciences",
                Grade = "Grade 5",
                TeacherNames = new() { "Demo Thabo Mokoena" }
            }
        }, x => x.Name + "|" + x.Grade);

        await AddMissing(store, "schoolDates", new[]
        {
            new SchoolDate
            {
                Title = "[DEMO] Application deadline",
                Date = today.AddDays(7),
                Description = "Sample date for testing only."
            },
            new SchoolDate
            {
                Title = "[DEMO] Parent meeting",
                Date = today.AddDays(14),
                Description = "Sample date for testing only."
            }
        }, x => x.Title);

        await AddMissing(store, "calendarEvents", new[]
        {
            new CalendarEvent
            {
                Title = "[DEMO] Staff planning",
                Start = today.AddDays(2).AddHours(8),
                End = today.AddDays(2).AddHours(9),
                Description = "Sample planning meeting for testing only."
            },
            new CalendarEvent
            {
                Title = "[DEMO] Sports afternoon",
                Start = today.AddDays(5).AddHours(12),
                End = today.AddDays(5).AddHours(14),
                Description = "Sample event for testing only."
            }
        }, x => x.Title);

        Console.WriteLine(
            "Demo data ready: applications, staff, classes, subjects, dates and calendar events.");
    }

    private static AdmissionApplication Application(
        string number,
        string first,
        string last,
        string grade,
        string status,
        int age,
        DateTime today) => new()
        {
            ApplicationNumber = number,
            LearnerFirstName = first,
            LearnerLastName = last,
            DateOfBirth = today.AddYears(-age),
            GradeApplyingFor = grade,
            GuardianName = "Demo Guardian " + last,
            GuardianEmail = number.ToLowerInvariant() + "@example.com",
            GuardianPhone = "",
            DateSubmitted = today.AddDays(-3),
            Status = status
        };

    private static async Task AddMissing<T>(
        FirestoreDataStore store,
        string collection,
        IEnumerable<T> samples,
        Func<T, string> key) where T : class
    {
        var existing = await store.GetAllAsync<T>(collection);

        var keys = existing
            .Select(key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var sample in samples)
        {
            if (keys.Add(key(sample)))
                await store.AddAsync(collection, sample);
        }
    }
}