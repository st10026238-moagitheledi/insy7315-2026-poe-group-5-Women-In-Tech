using System.Text.Json;
using KoosMatliApp.Services;
using Microsoft.Maui.Controls;

namespace KoosMatliApp.Views.Parent;

public partial class ParentDashboardPage : ContentPage
{
    private readonly ParentApiService _parentApiService = new();

    private bool _isLoading;

    public ParentDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoading)
            await LoadDashboardAsync();
    }

    // =============================================
    // LOAD DASHBOARD FROM THE PARENT API
    // =============================================

    private async Task LoadDashboardAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            RefreshButton.IsEnabled = false;
            StatusCard.IsVisible = false;

            using JsonDocument dashboard =
                await _parentApiService.GetDashboardAsync();

            JsonElement root = dashboard.RootElement;

            // Read common dashboard properties where available.
            // The exact property names depend on your API response.

            string? parentName = FindString(
                root,
                "displayName",
                "parentName",
                "fullName",
                "name");

            if (!string.IsNullOrWhiteSpace(parentName))
            {
                WelcomeLabel.Text = $"Welcome, {parentName}!";
            }

            string? schoolStatus = FindString(
                root,
                "schoolStatus",
                "schoolStatusMessage");

            if (!string.IsNullOrWhiteSpace(schoolStatus))
            {
                SchoolStatusLabel.Text = schoolStatus;
            }

            string? nextClass = FindString(
                root,
                "nextClass",
                "nextClassName",
                "nextLesson");

            if (!string.IsNullOrWhiteSpace(nextClass))
            {
                NextClassLabel.Text = nextClass;
            }

            string? nextClassTime = FindString(
                root,
                "nextClassTime",
                "nextLessonTime",
                "classTime");

            if (!string.IsNullOrWhiteSpace(nextClassTime))
            {
                NextClassTimeLabel.Text = nextClassTime;
            }

            string? announcement = FindString(
                root,
                "latestAnnouncement",
                "announcementTitle",
                "latestNotice");

            if (!string.IsNullOrWhiteSpace(announcement))
            {
                AnnouncementTitleLabel.Text = announcement;
            }

            string? announcementDate = FindString(
                root,
                "announcementDate",
                "publishedDate",
                "noticeDate");

            if (!string.IsNullOrWhiteSpace(announcementDate))
            {
                AnnouncementDateLabel.Text = announcementDate;
            }

            string? upcomingEvent = FindString(
                root,
                "upcomingEvent",
                "eventTitle",
                "nextEvent");

            if (!string.IsNullOrWhiteSpace(upcomingEvent))
            {
                UpcomingEventLabel.Text = upcomingEvent;
            }

            string? eventDate = FindString(
                root,
                "upcomingEventDate",
                "eventDate",
                "nextEventDate");

            if (!string.IsNullOrWhiteSpace(eventDate))
            {
                UpcomingEventDateLabel.Text = eventDate;
            }

            string? homework = FindString(
                root,
                "pendingHomework",
                "homeworkReminder",
                "latestHomework");

            if (!string.IsNullOrWhiteSpace(homework))
            {
                HomeworkLabel.Text = homework;
            }

            string? childrenSummary = FindString(
                root,
                "childrenSummary",
                "childrenCount",
                "numberOfChildren");

            if (!string.IsNullOrWhiteSpace(childrenSummary))
            {
                ChildrenSummaryLabel.Text =
                    $"Linked learners: {childrenSummary}";
            }

            System.Diagnostics.Debug.WriteLine(
                "Parent dashboard loaded successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowStatus(
                ex.Message +
                " Please return to the login screen and sign in again.");

            System.Diagnostics.Debug.WriteLine(
                $"Parent dashboard authorization error: {ex}");
        }
        catch (HttpRequestException ex)
        {
            ShowStatus(
                "Unable to load the latest school information. " +
                "Please check that the API is running and try again.");

            System.Diagnostics.Debug.WriteLine(
                $"Parent dashboard API error: {ex}");
        }
        catch (TaskCanceledException ex)
        {
            ShowStatus(
                "The request timed out. Please try refreshing the dashboard.");

            System.Diagnostics.Debug.WriteLine(
                $"Parent dashboard timeout: {ex}");
        }
        catch (Exception ex)
        {
            ShowStatus(
                "Something went wrong while loading the dashboard.");

            System.Diagnostics.Debug.WriteLine(
                $"Parent dashboard error: {ex}");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            RefreshButton.IsEnabled = true;

            _isLoading = false;
        }
    }

    // =============================================
    // FLEXIBLE JSON PROPERTY READER
    // =============================================

    private static string? FindString(
        JsonElement element,
        params string[] propertyNames)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in element.EnumerateObject())
            {
                foreach (string propertyName in propertyNames)
                {
                    if (string.Equals(
                        property.Name,
                        propertyName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        string? value = ReadValue(property.Value);

                        if (!string.IsNullOrWhiteSpace(value))
                            return value;
                    }
                }
            }

            // Search nested objects if the property isn't at the root.
            foreach (JsonProperty property in element.EnumerateObject())
            {
                string? value = FindString(
                    property.Value,
                    propertyNames);

                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in element.EnumerateArray())
            {
                string? value = FindString(item, propertyNames);

                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }
        }

        return null;
    }

    private static string? ReadValue(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.Number:
                return element.ToString();

            case JsonValueKind.True:
                return "Yes";

            case JsonValueKind.False:
                return "No";

            default:
                return null;
        }
    }

    // =============================================
    // STATUS MESSAGE
    // =============================================

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
        StatusCard.IsVisible = true;
    }

    // =============================================
    // REFRESH
    // =============================================

    private async void OnRefreshClicked(
        object sender,
        EventArgs e)
    {
        await LoadDashboardAsync();
    }

    // =============================================
    // NAVIGATION
    // =============================================

    private async Task ShowComingSoonAsync(string pageName)
    {
        await DisplayAlertAsync(
            pageName,
            $"{pageName} will be connected when we build that view.",
            "OK");
    }

    private async void OnHomeTapped(
        object sender,
        TappedEventArgs e)
    {
        // Already on the parent dashboard.
        await Task.CompletedTask;
    }

    private async void OnChildrenTapped(
        object sender,
        EventArgs e)
    {
        await Navigation.PushAsync(new MyChildrenPage());
    }

    private async void OnAnnouncementsTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync("Announcements");
    }

    private async void OnTimetableTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync("Timetables");
    }

    private async void OnHomeworkTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync(
            "Homework & Learning Resources");
    }

    private async void OnCalendarTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync(
            "School Calendar & Events");
    }

    private async void OnAcademicsTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync(
            "Academic Information");
    }

    private async void OnFeesTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync(
            "School Fees & Payments");
    }

    private async void OnProfileTapped(
        object sender,
        TappedEventArgs e)
    {
        await ShowComingSoonAsync(
            "Parent Profile & Settings");
    }
}
