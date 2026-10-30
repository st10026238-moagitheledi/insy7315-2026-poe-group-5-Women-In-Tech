using Google.Cloud.Firestore;
using System.Reflection;
using System.Globalization;
using KoosMatliAdminApi.Models;

namespace KoosMatliAdminApi.Services;

public sealed class FirestoreDataStore
{
    private readonly FirestoreDb _db;

    public FirestoreDataStore(FirestoreDb db)
    {
        _db = db;
    }

    public async Task<List<T>> GetAllAsync<T>(
        string collectionName) where T : class
    {
        var snapshot = await _db.Collection(collectionName)
            .GetSnapshotAsync(); (Google Cloud, 2026a).

        if (typeof(T) == typeof(User) && collectionName == "users")
        {
            return snapshot.Documents
                .Where(d => Active(d.ToDictionary()))
                .Select(d => (T)(object)ReadUser(d.Id, d.ToDictionary()))
                .ToList();
        }

        var records = snapshot.Documents
            .Select(d => d.ConvertTo<T>())
            .ToList();

        if (typeof(T) == typeof(StaffMember) &&
            collectionName == "staffMembers")
        {
            var staff = records.Cast<StaffMember>().ToList();

            var emails = staff
                .Select(x => x.Email.Trim())
                .Where(x => x.Length > 0)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var users = await GetAllAsync<User>("users");

            foreach (var teacher in users.Where(x => x.Role == "Teacher"))
            {
                if (teacher.Email.Length > 0 &&
                    !emails.Add(teacher.Email.Trim()))
                {
                    continue;
                }

                staff.Add(new StaffMember
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Email = teacher.Email,
                    Position = "Teacher"
                });
            }

            return staff.Cast<T>().ToList();
        }

        if (typeof(T) == typeof(CalendarEvent) &&
            collectionName == "calendarEvents")
        {
            var sharedEvents = await _db.Collection("events")
                .GetSnapshotAsync();

            records.AddRange(sharedEvents.Documents.Select(d =>
                (T)(object)ReadEvent(d.Id, d.ToDictionary())));
        }

        return records;
    }

    public async Task<List<User>> GetSchoolUsersAsync()
    {
        var profiles = await GetAllAsync<User>("users");
        var learners = await _db.Collection("learners").GetSnapshotAsync();

        if (learners.Count == 0)
            return profiles;

        // The learners collection represents school enrolments.
        profiles.RemoveAll(x => x.Role == "Learner");

        profiles.AddRange(learners.Documents
            .Where(d => Active(d.ToDictionary()))
            .Select(d =>
            {
                var learner = ReadUser(d.Id, d.ToDictionary());
                learner.Role = "Learner";
                return learner;
            }));

        return profiles;
    }

    public async Task<T?> GetByIdAsync<T>(
        string collectionName, int id) where T : class
    {
        var snapshot = await _db.Collection(collectionName)
            .Document(id.ToString(CultureInfo.InvariantCulture))
            .GetSnapshotAsync();

        if (!snapshot.Exists)
            return null;

        return typeof(T) == typeof(User) && collectionName == "users"
            ? (T)(object)ReadUser(snapshot.Id, snapshot.ToDictionary())
            : snapshot.ConvertTo<T>();
    }

    public static User ReadUser(
        string documentId,
        IReadOnlyDictionary<string, object> fields)
    {
        string first = Text(fields, "firstName", "FirstName");
        string last = Text(fields, "lastName", "LastName");

        if (first.Length == 0 && last.Length == 0)
        {
            var name = Text(fields, "displayName", "DisplayName")
                .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            first = name.FirstOrDefault() ?? "";
            last = name.Length > 1 ? name[1] : "";
        }

        int.TryParse(documentId, out int id);

        return new User
        {
            Id = id,
            FirstName = first,
            LastName = last,
            Email = Text(fields, "email", "Email"),
            PhoneNumber = Text(fields, "phoneNumber", "PhoneNumber"),
            Role = Text(fields, "role", "Role").ToLowerInvariant() switch
            {
                "admin" => "Admin",
                "teacher" => "Teacher",
                "parent" => "Parent",
                "learner" => "Learner",
                _ => ""
            }
        };
    }

    public static CalendarEvent ReadEvent(
        string documentId,
        IReadOnlyDictionary<string, object> fields)
    {
        var start = Date(fields, "Start", "start");
        var end = Date(fields, "End", "end");
        string time = Text(fields, "Time", "time");

        if (start is null)
        {
            start = Date(fields, "Date", "date")
                ?? throw new InvalidOperationException(
                    "Event has no valid Date: " + documentId);

            var times = time.Split(
                new[] { '-', '–', '—' },
                StringSplitOptions.TrimEntries);

            if (times.Length > 0 &&
                TimeSpan.TryParse(
                    times[0], CultureInfo.InvariantCulture, out var from) &&
                from >= TimeSpan.Zero &&
                from < TimeSpan.FromDays(1))
            {
                var localDate = new DateTimeOffset(start.Value)
                    .ToOffset(TimeSpan.FromHours(2)).Date;

                start = new DateTimeOffset(
                    localDate.Add(from),
                    TimeSpan.FromHours(2)).UtcDateTime;

                if (times.Length > 1 &&
                    TimeSpan.TryParse(
                        times[1], CultureInfo.InvariantCulture, out var to) &&
                    to >= TimeSpan.Zero &&
                    to < TimeSpan.FromDays(1))
                {
                    end = new DateTimeOffset(
                        localDate.Add(to),
                        TimeSpan.FromHours(2)).UtcDateTime;

                    if (end < start)
                        end = end.Value.AddDays(1);
                }
            }
        }

        int.TryParse(documentId, out int id);

        return new CalendarEvent
        {
            Id = id,
            Title = Text(fields, "Title", "title"),
            Start = start.Value,
            End = end,
            Description = string.Join("\n", new[]
            {
                Text(fields, "Description", "description"),
                Text(fields, "Location", "location"),
                time
            }.Where(x => x.Length > 0))
        };
    }

    private static bool Active(
        IReadOnlyDictionary<string, object> fields) =>
        !fields.TryGetValue("isActive", out var active) ||
        active is not false;

    private static string Text(
        IReadOnlyDictionary<string, object> fields,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (fields.TryGetValue(name, out var value) &&
                value is string text &&
                !string.IsNullOrWhiteSpace(text))
            {
                return text.Trim();
            }
        }

        return "";
    }

    private static DateTime? Date(
        IReadOnlyDictionary<string, object> fields,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (!fields.TryGetValue(name, out var value))
                continue;

            if (value is Timestamp timestamp)
                return timestamp.ToDateTime();

            if (value is DateTime date)
            {
                return date.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(date, DateTimeKind.Utc)
                    : date.ToUniversalTime();
            }

            if (value is string text &&
                DateTimeOffset.TryParse(
                    text,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out var parsed))
            {
                return parsed.UtcDateTime;
            }
        }

        return null;
    }

    public async Task<T?> GetFirstAsync<T>(
        string collectionName) where T : class
    {
        var records = await GetAllAsync<T>(collectionName);
        return records.OrderBy(GetId).FirstOrDefault();
    }

    public async Task<bool> IsEmptyAsync(string collectionName)
    {
        var snapshot = await _db.Collection(collectionName)
            .Limit(1).GetSnapshotAsync();

        return snapshot.Count == 0;
    }

    public async Task<T> AddAsync<T>(
        string collectionName,
        T entity,
        string? uniqueValue = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (collectionName == "calendarEvents" &&
            entity is CalendarEvent)
        {
            collectionName = "events";
        }

        var collection = _db.Collection(collectionName);
        var counter = _db.Collection("_metadata")
            .Document($"counter_{collectionName}");

        await _db.RunTransactionAsync(async transaction => //(Google Cloud, 2026b).
        {
            var snapshot = await transaction.GetSnapshotAsync(counter);

            long current = snapshot.Exists &&
                snapshot.ContainsField("value")
                    ? snapshot.GetValue<long>("value")
                    : 0;

            DocumentReference? unique = null;

            if (uniqueValue is not null)
            {
                var hash = Convert.ToHexString(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes( //(Microsoft Learn, 2026a).
                            uniqueValue.ToUpperInvariant())));

                unique = _db.Collection("_unique")
                    .Document(collectionName + "_" + hash);

                if ((await transaction.GetSnapshotAsync(unique)).Exists)
                    throw new DuplicateProfileException(); //(Microsoft Learn, 2026b).
            }

            DocumentReference document;

            do
            {
                current = checked(current + 1);

                if (current > int.MaxValue)
                    throw new InvalidOperationException(
                        "Integer IDs exhausted.");

                document = collection.Document(
                    current.ToString(CultureInfo.InvariantCulture));
            }
            while ((await transaction.GetSnapshotAsync(document)).Exists);

            SetId(entity, (int)current);

            transaction.Set(counter, new Dictionary<string, object>
            {
                ["value"] = current
            });

            if (collectionName == "events" &&
                entity is CalendarEvent calendar)
            {
                var local = new DateTimeOffset(calendar.Start)
                    .ToOffset(TimeSpan.FromHours(2));

                var endLocal = calendar.End.HasValue
                    ? new DateTimeOffset(calendar.End.Value)
                        .ToOffset(TimeSpan.FromHours(2))
                    : (DateTimeOffset?)null;

                transaction.Create(document,
                    new Dictionary<string, object?>
                    {
                        ["Id"] = calendar.Id,
                        ["Title"] = calendar.Title,
                        ["Description"] = calendar.Description,
                        ["Date"] = new DateTimeOffset(
                            local.Date,
                            TimeSpan.FromHours(2)).UtcDateTime,
                        ["Time"] = local.ToString("HH:mm") +
                            (endLocal.HasValue
                                ? " - " + endLocal.Value.ToString("HH:mm")
                                : ""),
                        ["Start"] = calendar.Start,
                        ["End"] = calendar.End,
                        ["Category"] = "General",
                        ["Location"] = "Koos Matli Primary School",
                        ["IsPublished"] = true
                    });
            }
            else
            {
                transaction.Create(document, entity);
            }

            if (unique is not null)
            {
                transaction.Create(unique,
                    new Dictionary<string, object>
                    {
                        ["Id"] = current
                    });
            }
        });

        return entity;
    }

    public async Task SaveAsync<T>(
        string collectionName, T entity) where T : class
    {
        ArgumentNullException.ThrowIfNull(entity);

        int id = GetId(entity);

        if (id <= 0)
        {
            throw new InvalidOperationException(
                $"A positive Id is required to save an item in '{collectionName}'.");
        }

        await _db.Collection(collectionName)
            .Document(id.ToString(CultureInfo.InvariantCulture))
            .SetAsync(entity, SetOptions.MergeAll);
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var document = _db.Collection("applications")
            .Document(id.ToString(CultureInfo.InvariantCulture));

        return await _db.RunTransactionAsync(async transaction => //(Google Cloud, 2026b).
        {
            if (!(await transaction.GetSnapshotAsync(document)).Exists)
                return false;

            transaction.Update(document, new Dictionary<string, object>
            {
                ["Status"] = status
            });

            return true;
        });
    }

    private static int GetId<T>(T entity)
    {
        var property = GetIdProperty(entity!.GetType());

        return Convert.ToInt32(
            property.GetValue(entity),
            CultureInfo.InvariantCulture);
    }

    private static void SetId<T>(T entity, int id) =>
        GetIdProperty(entity!.GetType()).SetValue(entity, id);

    private static PropertyInfo GetIdProperty(Type type)
    {
        var property = type.GetProperty(
            "Id", BindingFlags.Public | BindingFlags.Instance);

        if (property is null ||
            property.PropertyType != typeof(int) ||
            !property.CanRead ||
            !property.CanWrite)
        {
            throw new InvalidOperationException(
                $"{type.Name} must have a readable and writable integer Id property.");
        }

        return property;
    }
}

public sealed class DuplicateProfileException : Exception { }

/*Reference List
 
Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest

Google Cloud, "Transactions and Batched Writes in Cloud Firestore," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/firestore/docs/manage-data/transactions

Microsoft Corporation, "Reflection in .NET," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection/

Microsoft Corporation, "Exceptions and Exception Handling (C# Programming Guide)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/
 */