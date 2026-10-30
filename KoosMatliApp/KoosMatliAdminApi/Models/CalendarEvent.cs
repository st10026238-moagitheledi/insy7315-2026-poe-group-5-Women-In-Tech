using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class CalendarEvent
{
    public CalendarEvent() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;
    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime Start { get; set; }

    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime? End { get; set; }

    [FirestoreProperty]
    public string Description { get; set; } = string.Empty;
}
