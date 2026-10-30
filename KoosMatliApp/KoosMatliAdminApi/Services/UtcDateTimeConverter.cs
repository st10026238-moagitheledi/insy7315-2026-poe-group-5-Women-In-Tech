using Google.Cloud.Firestore;
namespace KoosMatliAdminApi.Services;
public sealed class UtcDateTimeConverter : IFirestoreConverter<DateTime> //(Google Cloud, 2026a).
{
    public object ToFirestore(DateTime value) => Timestamp.FromDateTime(value.Kind switch {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc) //(Google Cloud, 2026b; Microsoft Learn, 2026).
    });
    public DateTime FromFirestore(object value) => value is Timestamp timestamp
        ? timestamp.ToDateTime() : throw new ArgumentException("Expected a Firestore timestamp.");
}

/* Reference List
 Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest

 Google Cloud, "Firestore Data Types and Timestamps," Google Cloud Documentation, 2026. [Online]. Available: https://cloud.google.com/firestore/docs/concepts.datatype

 Microsoft Corporation, "DateTimeKind Enum (System)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.datetimekind
 
 */