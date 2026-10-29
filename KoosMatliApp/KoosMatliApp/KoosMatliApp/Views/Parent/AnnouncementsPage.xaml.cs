using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Parent;

public partial class AnnouncementsPage : ContentPage
{
    private readonly ParentApiService _parentApiService = new();

    private readonly ObservableCollection<AnnouncementItem>
        _allAnnouncements = new();

    public ObservableCollection<AnnouncementItem>
        FilteredAnnouncements
    { get; } = new();

    private bool _isLoading;
    private bool _hasLoadedSuccessfully;
    private string _selectedCategory = "All";

    public AnnouncementsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoading && !_hasLoadedSuccessfully)
        {
            await LoadAnnouncementsAsync();
        }
    }

    // =========================================
    // LOAD ANNOUNCEMENTS
    // =========================================

    private async Task LoadAnnouncementsAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;

            ShowLoadingOverlay();
            StatusContainer.IsVisible = false;

            using JsonDocument response =
                await _parentApiService.GetAnnouncementsAsync();

            JsonElement root = response.RootElement;

            if (TryGetProperty(
                    root,
                    out JsonElement successElement,
                    "success")
                && successElement.ValueKind == JsonValueKind.False)
            {
                ShowStatus(
                    GetString(root, "message")
                    ?? "Unable to load announcements.");

                _hasLoadedSuccessfully = false;
                return;
            }

            if (!TryGetAnnouncementsArray(
                    root,
                    out JsonElement announcementsElement))
            {
                ShowStatus(
                    "The server response did not contain a valid announcements list.");

                _hasLoadedSuccessfully = false;
                return;
            }

            _allAnnouncements.Clear();

            foreach (JsonElement announcementJson
                     in announcementsElement.EnumerateArray())
            {
                AnnouncementItem? announcement =
                    MapAnnouncement(announcementJson);

                if (announcement != null)
                {
                    _allAnnouncements.Add(announcement);
                }
            }

            // Display the newest announcements first.
            List<AnnouncementItem> sortedAnnouncements =
                _allAnnouncements
                    .OrderByDescending(item =>
                        item.PublishedDate == DateTime.MinValue
                            ? DateTime.MinValue
                            : item.PublishedDate)
                    .ToList();

            _allAnnouncements.Clear();

            foreach (AnnouncementItem item in sortedAnnouncements)
            {
                _allAnnouncements.Add(item);
            }

            ApplyFilters(AnnouncementSearchBar.Text ?? string.Empty);

            _hasLoadedSuccessfully = true;
            StatusContainer.IsVisible = false;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowStatus(
                $"{ex.Message} Please sign in again.");

            System.Diagnostics.Debug.WriteLine(
                $"Announcements authorization error: {ex}");

            _hasLoadedSuccessfully = false;
        }
        catch (HttpRequestException ex)
        {
            ShowStatus(
                "Unable to connect to the school server. " +
                "Check that your API is running and try again.");

            System.Diagnostics.Debug.WriteLine(
                $"Announcements API error: {ex}");

            _hasLoadedSuccessfully = false;
        }
        catch (TaskCanceledException ex)
        {
            ShowStatus(
                "The request timed out. Please try again.");

            System.Diagnostics.Debug.WriteLine(
                $"Announcements timeout: {ex}");

            _hasLoadedSuccessfully = false;
        }
        catch (JsonException ex)
        {
            ShowStatus(
                "The server returned data that could not be read.");

            System.Diagnostics.Debug.WriteLine(
                $"Announcements JSON error: {ex}");

            _hasLoadedSuccessfully = false;
        }
        catch (Exception ex)
        {
            ShowStatus(
                "Something went wrong while loading announcements.");

            System.Diagnostics.Debug.WriteLine(
                $"Announcements error: {ex}");

            _hasLoadedSuccessfully = false;
        }
        finally
        {
            HideLoadingOverlay();

            AnnouncementsRefreshView.IsRefreshing = false;

            _isLoading = false;
        }
    }

    private static bool TryGetAnnouncementsArray(
        JsonElement root,
        out JsonElement announcements)
    {
        if (TryGetProperty(
                root,
                out announcements,
                "announcements")
            && announcements.ValueKind == JsonValueKind.Array)
        {
            return true;
        }

        if (TryGetProperty(root, out JsonElement data, "data"))
        {
            if (data.ValueKind == JsonValueKind.Array)
            {
                announcements = data;
                return true;
            }

            if (TryGetProperty(
                    data,
                    out announcements,
                    "announcements")
                && announcements.ValueKind == JsonValueKind.Array)
            {
                return true;
            }

            if (TryGetProperty(
                    data,
                    out JsonElement nestedData,
                    "data")
                && TryGetProperty(
                    nestedData,
                    out announcements,
                    "announcements")
                && announcements.ValueKind == JsonValueKind.Array)
            {
                return true;
            }
        }

        if (root.ValueKind == JsonValueKind.Array)
        {
            announcements = root;
            return true;
        }

        announcements = default;
        return false;
    }

    // =========================================
    // MAP API JSON TO ANNOUNCEMENT MODEL
    // =========================================

    private static AnnouncementItem? MapAnnouncement(
        JsonElement json)
    {
        string? title = GetString(
            json,
            "title",
            "name",
            "announcementTitle");

        string? message = GetString(
            json,
            "message",
            "description",
            "content",
            "details",
            "body");

        string? summary = GetString(
            json,
            "summary",
            "excerpt");

        if (string.IsNullOrWhiteSpace(title))
            title = "School Announcement";

        if (string.IsNullOrWhiteSpace(message))
            message = summary ?? "No additional details available.";

        if (string.IsNullOrWhiteSpace(summary))
            summary = message;

        string? category = GetString(
            json,
            "category",
            "type");

        string? author = GetString(
            json,
            "author",
            "authorName",
            "createdByName",
            "postedBy",
            "createdBy");

        string? id = GetString(
            json,
            "id",
            "announcementId",
            "documentId");

        // Read the Firestore Timestamp or another supported date format.
        DateTime? parsedDate = ParseAnnouncementDate(json);

        DateTime publishedDate =
            parsedDate ?? DateTime.MinValue;

        return new AnnouncementItem
        {
            Id = id ?? string.Empty,
            Title = title,
            Category = NormalizeCategory(category),
            Message = message,
            Summary = summary,
            Author = string.IsNullOrWhiteSpace(author)
                ? "School Administration"
                : author,
            PublishedDate = publishedDate
        };
    }

    // =========================================
    // PARSE FIRESTORE TIMESTAMPS AND DATES
    // =========================================

    private static DateTime? ParseAnnouncementDate(
        JsonElement item)
    {
        string[] dateFields =
        {
            "createdAt",
            "datePublished",
            "publishedAt",
            "date",
            "timestamp"
        };

        foreach (string field in dateFields)
        {
            if (!TryGetProperty(
                    item,
                    out JsonElement value,
                    field))
            {
                continue;
            }

            // Case 1: A regular date string or ISO date string.
            if (value.ValueKind == JsonValueKind.String)
            {
                string? dateText = value.GetString();

                if (string.IsNullOrWhiteSpace(dateText))
                    continue;

                if (DateTimeOffset.TryParse(
                        dateText,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal,
                        out DateTimeOffset parsedDate))
                {
                    return parsedDate.LocalDateTime;
                }

                continue;
            }

            // Case 2: A numeric Unix timestamp.
            if (value.ValueKind == JsonValueKind.Number
                && value.TryGetInt64(out long timestamp))
            {
                try
                {
                    // Values below this threshold are treated as seconds.
                    if (timestamp < 100000000000L
                        && timestamp > -100000000000L)
                    {
                        return DateTimeOffset
                            .FromUnixTimeSeconds(timestamp)
                            .LocalDateTime;
                    }

                    // Larger values are treated as milliseconds.
                    return DateTimeOffset
                        .FromUnixTimeMilliseconds(timestamp)
                        .LocalDateTime;
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }

            // Case 3: Firestore Timestamp serialized as an object.
            // Example:
            // {
            //   "_seconds": 1792744200,
            //   "_nanoseconds": 0
            // }
            if (value.ValueKind == JsonValueKind.Object)
            {
                if (TryGetProperty(
                        value,
                        out JsonElement secondsElement,
                        "_seconds",
                        "seconds")
                    && secondsElement.TryGetInt64(out long seconds))
                {
                    long nanoseconds = 0;

                    if (TryGetProperty(
                            value,
                            out JsonElement nanosElement,
                            "_nanoseconds",
                            "nanoseconds"))
                    {
                        nanosElement.TryGetInt64(out nanoseconds);
                    }

                    try
                    {
                        return DateTimeOffset
                            .FromUnixTimeSeconds(seconds)
                            .AddTicks(nanoseconds / 100)
                            .LocalDateTime;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        continue;
                    }
                }

                // Case 4: Firestore Timestamp serialized as { seconds, nanoseconds }
                // is already covered above.
            }
        }

        // No usable date was found.
        return null;
    }

    private static string NormalizeCategory(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return "General";

        if (category.Contains(
                "academic",
                StringComparison.OrdinalIgnoreCase)
            || category.Contains(
                "homework",
                StringComparison.OrdinalIgnoreCase)
            || category.Contains(
                "exam",
                StringComparison.OrdinalIgnoreCase))
        {
            return "Academic";
        }

        if (category.Contains(
                "event",
                StringComparison.OrdinalIgnoreCase)
            || category.Contains(
                "calendar",
                StringComparison.OrdinalIgnoreCase))
        {
            return "Events";
        }

        return "General";
    }

    // =========================================
    // SEARCH
    // =========================================

    private void OnSearchTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilters(e.NewTextValue ?? string.Empty);
    }

    // =========================================
    // CATEGORY FILTERS
    // =========================================

    private void OnCategoryClicked(
        object sender,
        EventArgs e)
    {
        if (sender is not Button button
            || button.CommandParameter is not string category)
        {
            return;
        }

        _selectedCategory = category;

        UpdateCategoryButtonStyles();

        ApplyFilters(AnnouncementSearchBar.Text ?? string.Empty);
    }

    private void UpdateCategoryButtonStyles()
    {
        StyleCategoryButton(AllButton, "All");
        StyleCategoryButton(AcademicButton, "Academic");
        StyleCategoryButton(EventsButton, "Events");
        StyleCategoryButton(GeneralButton, "General");
    }

    private void StyleCategoryButton(
        Button button,
        string category)
    {
        bool selected = _selectedCategory == category;

        button.BackgroundColor = selected
            ? Color.FromArgb("#061D45")
            : Colors.White;

        button.TextColor = selected
            ? Colors.White
            : Color.FromArgb("#061D45");

        button.BorderColor = selected
            ? Color.FromArgb("#061D45")
            : Color.FromArgb("#E0E7F1");
    }

    private void ApplyFilters(string searchText)
    {
        FilteredAnnouncements.Clear();

        IEnumerable<AnnouncementItem> results = _allAnnouncements;

        if (_selectedCategory != "All")
        {
            results = results.Where(item =>
                item.Category.Equals(
                    _selectedCategory,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            results = results.Where(item =>
                item.Title.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase)
                || item.Message.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase)
                || item.Category.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase)
                || item.Author.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        foreach (AnnouncementItem item in results)
        {
            FilteredAnnouncements.Add(item);
        }

        AnnouncementCountLabel.Text =
            $"{_allAnnouncements.Count} " +
            (_allAnnouncements.Count == 1
                ? "Announcement"
                : "Announcements");

        ResultsCountLabel.Text =
            $"{FilteredAnnouncements.Count} " +
            (FilteredAnnouncements.Count == 1
                ? "update"
                : "updates");
    }

    // =========================================
    // REFRESH
    // =========================================

    private async void OnRefreshing(
        object sender,
        EventArgs e)
    {
        await LoadAnnouncementsAsync();
    }

    private async void OnRefreshClicked(
        object sender,
        TappedEventArgs e)
    {
        await LoadAnnouncementsAsync();
    }

    // =========================================
    // OPEN ANNOUNCEMENT DETAILS
    // =========================================

    private async void OnAnnouncementTapped(
        object sender,
        TappedEventArgs e)
    {
        if (e.Parameter is not AnnouncementItem item)
            return;

        string date = item.PublishedDate == DateTime.MinValue
            ? "Date not available"
            : item.PublishedDate.ToString("dd MMM yyyy");

        await DisplayAlertAsync(
            item.Title,
            $"{item.Category}\n" +
            $"Published: {date}\n" +
            $"By: {item.Author}\n\n" +
            item.Message,
            "Close");
    }

    // =========================================
    // LOADING OVERLAY
    // =========================================

    private void ShowLoadingOverlay()
    {
        LoadingOverlay.IsVisible = true;
        OverlayActivityIndicator.IsRunning = true;
    }

    private void HideLoadingOverlay()
    {
        OverlayActivityIndicator.IsRunning = false;
        LoadingOverlay.IsVisible = false;
    }

    // =========================================
    // STATUS MESSAGE
    // =========================================

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
        StatusContainer.IsVisible = true;
    }

    // =========================================
    // JSON HELPERS
    // =========================================

    private static string? GetString(
        JsonElement element,
        params string[] names)
    {
        if (!TryGetProperty(
                element,
                out JsonElement value,
                names))
        {
            return null;
        }

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.ToString(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => null
        };
    }

    private static bool TryGetProperty(
        JsonElement element,
        out JsonElement value,
        params string[] names)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (names.Any(name =>
                    string.Equals(
                        property.Name,
                        name,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    // =========================================
    // BOTTOM NAVIGATION
    // =========================================

    private async void OnHomeTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentDashboardPage());
    }

    private async void OnScheduleTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new TimetablePage());
    }

    private async void OnUpdatesTapped(
        object sender,
        TappedEventArgs e)
    {
        // Already on the Announcements page.
        await Task.CompletedTask;
    }

    private async void OnChildrenTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyChildrenPage());
    }

    private async void OnProfileTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentProfilePage());
    }
}

// =============================================
// ANNOUNCEMENT MODEL
// =============================================

public class AnnouncementItem : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _category = "General";
    private string _message = string.Empty;
    private string _summary = string.Empty;
    private string _author = "School Administration";
    private DateTime _publishedDate = DateTime.MinValue;

    public string Id { get; set; } = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            if (_title == value)
                return;

            _title = value;
            OnPropertyChanged();
        }
    }

    public string Category
    {
        get => _category;
        set
        {
            if (_category == value)
                return;

            _category = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            if (_message == value)
                return;

            _message = value;
            OnPropertyChanged();
        }
    }

    public string Summary
    {
        get => _summary;
        set
        {
            if (_summary == value)
                return;

            _summary = value;
            OnPropertyChanged();
        }
    }

    public string Author
    {
        get => _author;
        set
        {
            if (_author == value)
                return;

            _author = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AuthorDisplay));
        }
    }

    public string AuthorDisplay => $"Posted by {Author}";

    public DateTime PublishedDate
    {
        get => _publishedDate;
        set
        {
            if (_publishedDate == value)
                return;

            _publishedDate = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DateDisplay));
        }
    }

    public string DateDisplay =>
        PublishedDate == DateTime.MinValue
            ? "Date not available"
            : PublishedDate.ToString("dd MMM yyyy");

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
