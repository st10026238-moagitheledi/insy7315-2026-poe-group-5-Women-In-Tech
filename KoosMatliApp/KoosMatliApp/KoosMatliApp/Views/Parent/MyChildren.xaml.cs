using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Parent;

public partial class MyChildrenPage : ContentPage
{
    private readonly ParentApiService _parentApiService = new();

    private readonly ObservableCollection<ChildItem> _allChildren = new();

    public ObservableCollection<ChildItem> FilteredChildren { get; } = new();

    private bool _isLoading;

    public MyChildrenPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoading)
        {
            await LoadChildrenAsync();
        }
    }

    // =========================================
    // LOAD CHILDREN FROM THE API
    // =========================================

    private async Task LoadChildrenAsync()
    {
        if (_isLoading)
            return;

        try
        {
            _isLoading = true;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            ChildrenCollectionView.IsVisible = false;
            StatusLabel.IsVisible = false;

            using JsonDocument response =
                await _parentApiService.GetChildrenAsync();

            JsonElement root = response.RootElement;

            bool success = TryGetProperty(
                root, out JsonElement successElement, "success")
                && successElement.ValueKind == JsonValueKind.True;

            if (!success)
            {
                ShowStatus(GetString(
                    root, "message")
                    ?? "Unable to retrieve your children.");

                return;
            }

            if (!TryGetProperty(
                    root, out JsonElement childrenElement, "children")
                || childrenElement.ValueKind != JsonValueKind.Array)
            {
                ShowStatus(
                    "The server response did not contain a valid children list.");

                return;
            }

            _allChildren.Clear();

            foreach (JsonElement childJson in
                     childrenElement.EnumerateArray())
            {
                var child = MapChild(childJson);

                if (child != null)
                {
                    _allChildren.Add(child);
                }
            }

            ApplyFilter(ChildSearchBar.Text ?? string.Empty);

            ChildrenCollectionView.IsVisible = true;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowStatus(
                $"{ex.Message} Return to login and sign in again.");
        }
        catch (HttpRequestException ex)
        {
            ShowStatus(
                "Unable to connect to the school server. " +
                "Check that your API is running and try again.");

            System.Diagnostics.Debug.WriteLine(
                $"My Children API error: {ex}");
        }
        catch (TaskCanceledException ex)
        {
            ShowStatus(
                "The request timed out. Please try again.");

            System.Diagnostics.Debug.WriteLine(
                $"My Children timeout: {ex}");
        }
        catch (Exception ex)
        {
            ShowStatus(
                "Something went wrong while loading your children.");

            System.Diagnostics.Debug.WriteLine(
                $"My Children error: {ex}");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            _isLoading = false;
        }
    }

    // =========================================
    // MAP FIRESTORE JSON TO A LEARNER
    // =========================================


    private static ChildItem? MapChild(JsonElement json)
    {
        string? id = GetString(json,
            "id", "childId", "uid", "learnerId", "studentId");

        string? firstName = GetString(json,
            "firstName", "firstname", "givenName");

        string? lastName = GetString(json,
            "lastName", "lastname", "surname", "familyName");

        string? name = GetString(json,
            "fullName", "displayName", "name",
            "learnerName", "studentName");

        if (string.IsNullOrWhiteSpace(name))
        {
            name = string.Join(" ",
                new[] { firstName, lastName }
                    .Where(part => !string.IsNullOrWhiteSpace(part)));
        }

        if (string.IsNullOrWhiteSpace(name))
            return null;

        string? grade = GetString(json,
            "grade", "gradeName", "gradeLevel",
            "className", "class", "currentGrade");

        string? status = GetString(json,
            "status", "learnerStatus", "enrolmentStatus",
            "enrollmentStatus");

        return new ChildItem
        {
            Id = id ?? string.Empty,
            FullName = name.Trim(),
            Grade = string.IsNullOrWhiteSpace(grade)
                ? "Grade not specified"
                : grade,
            Status = string.IsNullOrWhiteSpace(status)
                ? "Linked Learner"
                : status
        };
    }


    private static string? GetString(
        JsonElement element,
        params string[] names)
    {
        if (!TryGetProperty(element, out JsonElement value, names))
            return null;

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.ToString(),
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
            foreach (string name in names)
            {
                if (element.TryGetProperty(name, out value))
                    return true;
            }

            // Also support property names with different casing.
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (names.Any(name => string.Equals(
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
    // SEARCH
    // =========================================

    private void OnChildSearchTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue ?? string.Empty);
    }

    private void ApplyFilter(string searchText)
    {
        FilteredChildren.Clear();

        IEnumerable<ChildItem> results = _allChildren;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            results = _allChildren.Where(child =>
                child.FullName.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase)
                ||
                child.Grade.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        foreach (ChildItem child in results)
        {
            FilteredChildren.Add(child);
        }

        ChildrenCountLabel.Text =
            $"{_allChildren.Count} " +
            (_allChildren.Count == 1 ? "Child" : "Children");

        ResultsCountLabel.Text =
            $"{FilteredChildren.Count} " +
            (FilteredChildren.Count == 1 ? "profile" : "profiles");
    }

    // =========================================
    // SELECT A CHILD
    // =========================================

    private async void OnChildTapped(
        object sender,
        TappedEventArgs e)
    {
        if (e.Parameter is not ChildItem child)
            return;

        await DisplayAlertAsync(
            child.FullName,
            $"Learner ID: {child.Id}\n" +
            $"Grade: {child.Grade}\n" +
            $"Status: {child.Status}",
            "OK");
    }

    // =========================================
    // STATUS MESSAGES
    // =========================================

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
        StatusLabel.IsVisible = true;
        ChildrenCollectionView.IsVisible = false;
    }

    private static string? GetString(
        JsonElement element,
        string name)
    {
        return TryGetProperty(element, out JsonElement value, name)
            && value.ValueKind == JsonValueKind.String
                ? value.GetString()
                : null;
    }

    // =========================================
    // BOTTOM NAVIGATION
    // =========================================

    private async void OnHomeTapped(
        object sender,
        TappedEventArgs e)
    {
        await ReturnToDashboardAsync();
    }

    private async void OnChildrenTapped(
        object sender,
        TappedEventArgs e)
    {
        // Already on this page.
        await Task.CompletedTask;
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
        await Navigation.PushAsync(new AnnouncementsPage());
    }

    private async void OnProfileTapped(
        object sender,
        TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentProfilePage());
    }

    private async Task ReturnToDashboardAsync()
    {
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
        else
        {
            await Navigation.PushAsync(new ParentDashboardPage());
        }
    }

    private async Task ShowComingSoonAsync(string pageName)
    {
        await DisplayAlertAsync(
            pageName,
            $"{pageName} will be connected when that page is created.",
            "OK");
    }
}

// =============================================
// LEARNER MODEL
// =============================================

public class ChildItem : INotifyPropertyChanged
{
    private string _fullName = string.Empty;
    private string _grade = string.Empty;
    private string _status = string.Empty;

    public string Id { get; set; } = string.Empty;

    public string FullName
    {
        get => _fullName;
        set
        {
            if (_fullName == value) return;

            _fullName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Initials));
        }
    }

    public string Grade
    {
        get => _grade;
        set
        {
            if (_grade == value) return;

            _grade = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            if (_status == value) return;

            _status = value;
            OnPropertyChanged();
        }
    }

    public string Initials
    {
        get
        {
            string[] names = FullName.Split(
                ' ', StringSplitOptions.RemoveEmptyEntries);

            if (names.Length == 0) return "?";

            if (names.Length == 1)
                return names[0][0].ToString().ToUpperInvariant();

            return $"{names[0][0]}{names[^1][0]}"
                .ToUpperInvariant();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
