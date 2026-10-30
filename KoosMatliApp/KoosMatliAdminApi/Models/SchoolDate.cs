using Google.Cloud.Firestore;

namespace KoosMatliAdminApi.Models;

[FirestoreData]
public class SchoolDate
{
    public SchoolDate() { }

    [FirestoreProperty]
    public int Id { get; set; }

    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;
    [FirestoreProperty(ConverterType = typeof(KoosMatliAdminApi.Services.UtcDateTimeConverter))]
    public DateTime Date { get; set; }

    [FirestoreProperty]
    public string Description { get; set; } = string.Empty;
}
