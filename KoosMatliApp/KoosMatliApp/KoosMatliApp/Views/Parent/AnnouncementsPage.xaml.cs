using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using KoosMatliApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KoosMatliApp.Views.Parent;

public partial class AnnouncementsPage : ContentPage
{
    private readonly ParentApiService _parentApiService;

    private readonly ObservableCollection<AnnouncementItem>
        _allAnnouncements = new();

    public ObservableCollection<AnnouncementItem>
        FilteredAnnouncements
    { get; } = new();

    private string _selectedCategory = "All";
    private bool _isLoading;
    private bool _loadFailed;

    public AnnouncementsPage()
    {
        InitializeComponent();

        BindingContext = this;

        _parentApiService =
            Application.Current!.Handler!.MauiContext!
                .Services.GetRequiredService<ParentApiService>();

        Loaded += async (_, _) => await LoadAnnouncementsAsync();
    }

    private async void OnRefreshClicked(
        object sender,
        EventArgs e)
    {
        await LoadAnnouncementsAsync();
    }

    private async void OnRefreshing(
        object sender,
        EventArgs e)
    {
        await LoadAnnouncementsAsync();
    }

    private async Task LoadAnnouncementsAsync()
    {
        if (_isLoading)
            return;

        _isLoading = true;

        AnnouncementsRefreshView.IsRefreshing = true;
        LoadingOverlay.IsVisible = true;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        StatusContainer.IsVisible = false;
        _loadFailed = false;

        AnnouncementCountLabel.Text =
            "Loading announcements...";

        try
        {
            using JsonDocument document =
                await _parentApiService.GetAnnouncementsAsync();

            JsonElement root = document.RootElement;

            if (root.ValueKind == JsonValueKind.Object &&
                root.TryGetProperty(
                    "success", out JsonElement success) &&
                success.ValueKind == JsonValueKind.False)
            {
                throw new HttpRequestException(
                    ReadString(root, "message") ??
                    "Unable to load announcements.");
            }

            JsonElement announcements =
                GetProperty(root, "announcements");

            // Support APIs that wrap results in a data object.
            if (announcements.ValueKind != JsonValueKind.Array)
            {
                JsonElement data = GetProperty(root, "data");

                announcements =
                    GetProperty(data, "announcements");
            }

            if (announcements.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException(
                    "The API response does not contain an announcements array.");
            }

            _allAnnouncements.Clear();

            foreach (JsonElement item in
                     announcements.EnumerateArray())
            {
                AnnouncementItem? announcement =
                    ParseAnnouncement(item);

                if (announcement != null)
                    _allAnnouncements.Add(announcement);
            }

            ApplyFilters();

            StatusContainer.IsVisible = false;
            _loadFailed = false;

            AnnouncementCountLabel.Text =
                _allAnnouncements.Count == 1
                    ? "1 published announcement"
                    : $"{_allAnnouncements.Count} published announcements";
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowError(
                "Your session has expired. Please sign in again.");

            await DisplayAlertAsync(
                "Session expired",
                ex.Message,
                "OK");
        }
        catch (HttpRequestException ex)
        {
            ShowError(ex.Message);
        }
        catch (TaskCanceledException)
        {
            ShowError(
                "The request timed out. Check your connection and try again.");
        }
        catch (JsonException ex)
        {
            ShowError(
                $"Unable to read announcements: {ex.Message}");
        }
        catch (Exception ex)
        {
            ShowError(
                $"Unable to load announcements: {ex.Message}");
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            AnnouncementsRefreshView.IsRefreshing = false;
            _isLoading = false;
        }
    }

    private void ShowError(string message)
    {
        _loadFailed = true;

        StatusLabel.Text = message;
        StatusContainer.IsVisible = true;

        AnnouncementCountLabel.Text =
            "Announcements could not be loaded";

        // Do not present old data as if it were current.
        _allAnnouncements.Clear();
        FilteredAnnouncements.Clear();
    }

    private static AnnouncementItem? ParseAnnouncement(
        JsonElement item)
    {
        if (item.ValueKind != JsonValueKind.Object)
            return null;

        string title =
            ReadString(item, "title") ??
            ReadString(item, "name") ??
            "School announcement";

        string details =
            ReadString(item, "message") ??
            ReadString(item, "description") ??
            ReadString(item, "content") ??
            ReadString(item, "details") ??
            "";

        string summary =
            ReadString(item, "summary") ??
            details;

        string category =
            ReadString(item, "category") ??
            ReadString(item, "type") ??
            "General";

        string author =
            ReadString(item, "author") ??
            ReadString(item, "createdByName") ??
            ReadString(item, "postedBy") ??
            "School Administration";

        string? rawDate =
            ReadString(item, "datePublished") ??
            ReadString(item, "publishedAt") ??
            ReadString(item, "createdAt") ??
            ReadString(item, "date");

        DateTime publishedDate = DateTime.MinValue;

        if (!string.IsNullOrWhiteSpace(rawDate))
        {
            DateTime.TryParse(
                rawDate,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out publishedDate);
        }

        return new AnnouncementItem
        {
            Id =
                ReadString(item, "id") ??
                ReadString(item, "announcementId") ??
                ReadString(item, "documentId") ??
                "",

            Title = title,
            Category = NormalizeCategory(category),
            Summary = string.IsNullOrWhiteSpace(summary)
                ? "Open this announcement for more information."
                : summary,
            Details = string.IsNullOrWhiteSpace(details)
                ? summary
                : details,
            DatePublished = publishedDate,
            Author = author
        };
    }

    private static string NormalizeCategory(string category)
    {
        string value = category.Trim();

        if (value.Equals(
                "Academic",
                StringComparison.OrdinalIgnoreCase))
            return "Academic";

        if (value.Equals(
                "Event",
                StringComparison.OrdinalIgnoreCase) ||
            value.Equals(
                "Events",
                StringComparison.OrdinalIgnoreCase))
            return "Events";

        return "General";
    }

    private void ApplyFilters()
    {
        string searchText =
            AnnouncementSearchBar?.Text?.Trim() ?? "";

        var results = _allAnnouncements
            .Where(item =>
            {
                bool matchesCategory =
                    _selectedCategory == "All" ||
                    string.Equals(
                        item.Category,
                        _selectedCategory,
                        StringComparison.OrdinalIgnoreCase);

                bool matchesSearch =
                    string.IsNullOrWhiteSpace(searchText) ||
                    item.Title.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase) ||
                    item.Summary.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase) ||
                    item.Details.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase) ||
                    item.Category.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase) ||
                    item.Author.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase);

                return matchesCategory && matchesSearch;
            })
            .OrderByDescending(item => item.DatePublished)
            .ToList();

        FilteredAnnouncements.Clear();

        foreach (AnnouncementItem item in results)
            FilteredAnnouncements.Add(item);

        if (!_loadFailed)
        {
            AnnouncementCountLabel.Text =
                $"{FilteredAnnouncements.Count} announcement(s)";
        }
    }

    private void OnSearchTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnCategoryClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not string category)
        {
            return;
        }

        _selectedCategory = category;

        ResetCategoryButtons();

        button.BackgroundColor = Color.FromArgb("#061D45");
        button.TextColor = Colors.White;

        ApplyFilters();
    }

    private void ResetCategoryButtons()
    {
        Button[] buttons =
        {
            AllButton,
            AcademicButton,
            EventsButton,
            GeneralButton
        };

        foreach (Button button in buttons)
        {
            button.BackgroundColor = Colors.White;
            button.TextColor = Color.FromArgb("#061D45");
        }
    }

    private async void OnAnnouncementTapped(
        object sender,
        TappedEventArgs e)
    {
        if (e.Parameter is not AnnouncementItem announcement)
            return;

        await DisplayAlertAsync(
            announcement.Title,
            $"{announcement.Category} · {announcement.DateDisplay}\n\n" +
            $"{announcement.Details}\n\n" +
            $"Posted by: {announcement.Author}",
            "Close");
    }

    private static JsonElement GetProperty(
        JsonElement element,
        string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object)
            return default;

        if (element.TryGetProperty(
                propertyName, out JsonElement value))
            return value;

        // Also support camelCase/PascalCase differences.
        foreach (JsonProperty property in
                 element.EnumerateObject())
        {
            if (string.Equals(
                    property.Name,
                    propertyName,
                    StringComparison.OrdinalIgnoreCase))
            {
                return property.Value;
            }
        }

        return default;
    }

    private static string? ReadString(
        JsonElement element,
        string propertyName)
    {
        JsonElement value = GetProperty(element, propertyName);

        if (value.ValueKind == JsonValueKind.String)
            return value.GetString();

        if (value.ValueKind == JsonValueKind.Number)
            return value.ToString();

        return null;
    }
}

public class AnnouncementItem
{
    public string Id { get; set; } = "";

    public string Title { get; set; } = "";

    public string Category { get; set; } = "General";

    public string Summary { get; set; } = "";

    public string Details { get; set; } = "";

    public DateTime DatePublished { get; set; }

    public string Author { get; set; } =
        "School Administration";

    public string DateDisplay =>
        DatePublished == DateTime.MinValue
            ? "Date unavailable"
            : DatePublished.ToString("dd MMM yyyy");

    public string AuthorDisplay => $"Posted by {Author}";
}
