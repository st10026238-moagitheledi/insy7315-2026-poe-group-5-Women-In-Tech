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
        await LoadChildrenAsync();
    }

    // =========================================
    // LOAD LEARNERS
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
            StatusLabel.IsVisible = false;

            using JsonDocument response =
                await _parentApiService.GetChildrenAsync();

            JsonElement root = response.RootElement;

            if (TryGetProperty(root, out JsonElement success, "success")
                && success.ValueKind == JsonValueKind.False)
            {
                ShowStatus(GetString(root, "message")
                    ?? "Unable to load learner profiles.");
                return;
            }

            if (!TryGetChildrenArray(root, out JsonElement childrenArray))
            {
                ShowStatus("The server response did not contain a valid learner list.");
                return;
            }

            _allChildren.Clear();

            foreach (JsonElement childJson in childrenArray.EnumerateArray())
            {
                ChildItem? child = MapChild(childJson);

                if (child != null)
                    _allChildren.Add(child);
            }

            ApplySearch(ChildSearchBar.Text ?? string.Empty);
            StatusLabel.IsVisible = false;
        }
        catch (UnauthorizedAccessException ex)
        {
            ShowStatus($"{ex.Message} Please sign in again.");
        }
        catch (HttpRequestException ex)
        {
            ShowStatus("Unable to connect to the school server. Check that your API is running and try again.");
            System.Diagnostics.Debug.WriteLine($"MyChildren API error: {ex}");
        }
        catch (TaskCanceledException ex)
        {
            ShowStatus("The request timed out. Please try again.");
            System.Diagnostics.Debug.WriteLine($"MyChildren timeout: {ex}");
        }
        catch (JsonException ex)
        {
            ShowStatus("The server returned learner data that could not be read.");
            System.Diagnostics.Debug.WriteLine($"MyChildren JSON error: {ex}");
        }
        catch (Exception ex)
        {
            ShowStatus("Something went wrong while loading learner profiles.");
            System.Diagnostics.Debug.WriteLine($"MyChildren error: {ex}");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            _isLoading = false;
        }
    }

    private static bool TryGetChildrenArray(
        JsonElement root,
        out JsonElement children)
    {
        if (TryGetProperty(root, out children, "children")
            && children.ValueKind == JsonValueKind.Array)
        {
            return true;
        }

        if (TryGetProperty(root, out JsonElement data, "data"))
        {
            if (TryGetProperty(data, out children, "children")
                && children.ValueKind == JsonValueKind.Array)
            {
                return true;
            }

            if (data.ValueKind == JsonValueKind.Array)
            {
                children = data;
                return true;
            }
        }

        children = default;
        return false;
    }

    // =========================================
    // MAP API DATA
    // =========================================

    private static ChildItem? MapChild(JsonElement json)
    {
        string? firstName = GetString(json, "firstName", "givenName");
        string? lastName = GetString(json, "lastName", "surname", "familyName");

        string? fullName = GetString(json, "fullName", "name", "learnerName");

        if (string.IsNullOrWhiteSpace(fullName))
            fullName = $"{firstName} {lastName}".Trim();

        if (string.IsNullOrWhiteSpace(fullName))
            fullName = "Learner";

        string? grade = GetString(json, "grade", "gradeName", "yearLevel");
        string? className = GetString(json, "className", "classroom", "classCode", "class");

        string? admissionNumber = GetString(
            json, "admissionNumber", "learnerNumber", "studentNumber",
            "studentId", "learnerId");

        string? dateOfBirth = GetString(
            json, "dateOfBirth", "dob", "birthDate");

        string? gender = GetString(json, "gender", "sex");

        string? learnerContact = GetString(
            json, "learnerContactNumber", "learnerPhone",
            "contactNumber", "phoneNumber");

        string? learnerEmail = GetString(
            json, "learnerEmail", "email");

        string? address = GetString(
            json, "address", "homeAddress", "residentialAddress");

        string? parentName = GetString(
            json, "parentName", "parentFullName");

        string? parentContact = GetString(
            json, "parentContactNumber", "parentPhoneNumber",
            "parentPhone", "contactNumberParent");

        string? parentEmail = GetString(
            json, "parentEmail", "parentEmailAddress");

        string? guardianName = GetString(
            json, "guardianName", "guardianFullName");

        string? guardianContact = GetString(
            json, "guardianContactNumber", "guardianPhoneNumber",
            "guardianPhone");

        string? guardianEmail = GetString(
            json, "guardianEmail", "guardianEmailAddress");

        string? emergencyName = GetString(
            json, "emergencyContactName");

        string? emergencyContact = GetString(
            json, "emergencyContactNumber");

        string? medicalInformation = GetString(
            json, "medicalInformation", "medicalInfo");

        string? allergies = GetString(json, "allergies");

        string? status = GetString(json, "status", "learnerStatus");

        if (string.IsNullOrWhiteSpace(status))
        {
            bool isActive = !TryGetProperty(json, out JsonElement active, "isActive")
                || active.ValueKind != JsonValueKind.False;

            status = isActive ? "Active" : "Inactive";
        }

        return new ChildItem
        {
            Id = GetString(json, "id", "childId", "learnerId") ?? string.Empty,
            FullName = fullName,
            Grade = grade ?? string.Empty,
            ClassName = className ?? string.Empty,
            AdmissionNumber = admissionNumber ?? string.Empty,
            DateOfBirth = FormatDate(dateOfBirth),
            Gender = gender ?? string.Empty,
            LearnerContactNumber = learnerContact ?? string.Empty,
            LearnerEmail = learnerEmail ?? string.Empty,
            Address = address ?? string.Empty,
            ParentName = parentName ?? string.Empty,
            ParentContactNumber = parentContact ?? string.Empty,
            ParentEmail = parentEmail ?? string.Empty,
            GuardianName = guardianName ?? string.Empty,
            GuardianContactNumber = guardianContact ?? string.Empty,
            GuardianEmail = guardianEmail ?? string.Empty,
            EmergencyContactName = emergencyName ?? string.Empty,
            EmergencyContactNumber = emergencyContact ?? string.Empty,
            MedicalInformation = medicalInformation ?? string.Empty,
            Allergies = allergies ?? string.Empty,
            Status = status
        };
    }

    private static string FormatDate(string? dateText)
    {
        if (string.IsNullOrWhiteSpace(dateText))
            return string.Empty;

        if (DateTime.TryParse(dateText, out DateTime date))
            return date.ToString("dd MMM yyyy");

        return dateText;
    }

    // =========================================
    // SEARCH
    // =========================================

    private void OnChildSearchTextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        ApplySearch(e.NewTextValue ?? string.Empty);
    }

    private void ApplySearch(string searchText)
    {
        FilteredChildren.Clear();

        IEnumerable<ChildItem> results = _allChildren;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            results = results.Where(child =>
                child.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || child.Grade.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || child.ClassName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || child.AdmissionNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (ChildItem child in results)
            FilteredChildren.Add(child);

        ChildrenCountLabel.Text =
            $"{_allChildren.Count} " +
            (_allChildren.Count == 1 ? "Child" : "Children");

        ResultsCountLabel.Text =
            $"{FilteredChildren.Count} " +
            (FilteredChildren.Count == 1 ? "profile" : "profiles");
    }

    // =========================================
    // VIEW FULL LEARNER PROFILE
    // =========================================

    private async void OnChildTapped(
        object sender,
        TappedEventArgs e)
    {
        if (e.Parameter is not ChildItem child)
            return;

        string details =
            $"LEARNER INFORMATION\n" +
            $"Full name: {child.FullName}\n" +
            $"Grade: {DisplayValue(child.Grade)}\n" +
            $"Class: {DisplayValue(child.ClassName)}\n" +
            $"Admission number: {DisplayValue(child.AdmissionNumber)}\n" +
            $"Date of birth: {DisplayValue(child.DateOfBirth)}\n" +
            $"Gender: {DisplayValue(child.Gender)}\n" +
            $"Status: {DisplayValue(child.Status)}\n\n" +

            $"LEARNER CONTACT\n" +
            $"Phone: {DisplayValue(child.LearnerContactNumber)}\n" +
            $"Email: {DisplayValue(child.LearnerEmail)}\n" +
            $"Address: {DisplayValue(child.Address)}\n\n" +

            $"PARENT / GUARDIAN\n" +
            $"Parent: {DisplayValue(child.ParentName)}\n" +
            $"Parent phone: {DisplayValue(child.ParentContactNumber)}\n" +
            $"Parent email: {DisplayValue(child.ParentEmail)}\n" +
            $"Guardian: {DisplayValue(child.GuardianName)}\n" +
            $"Guardian phone: {DisplayValue(child.GuardianContactNumber)}\n" +
            $"Guardian email: {DisplayValue(child.GuardianEmail)}\n\n" +

            $"EMERGENCY CONTACT\n" +
            $"Name: {DisplayValue(child.EmergencyContactName)}\n" +
            $"Phone: {DisplayValue(child.EmergencyContactNumber)}\n\n" +

            $"ADDITIONAL INFORMATION\n" +
            $"Medical information: {DisplayValue(child.MedicalInformation)}\n" +
            $"Allergies: {DisplayValue(child.Allergies)}";

        await DisplayAlertAsync(child.FullName, details, "Close");
    }

    private static string DisplayValue(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "Not provided" : value;

    // =========================================
    // REFRESH
    // =========================================

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadChildrenAsync();
    }

    // =========================================
    // STATUS MESSAGE
    // =========================================

    private void ShowStatus(string message)
    {
        StatusLabel.Text = message;
        StatusLabel.IsVisible = true;
    }

    // =========================================
    // JSON HELPERS
    // =========================================

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
    // BOTTOM NAVIGATION
    // =========================================

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentDashboardPage());
    }

    private async void OnScheduleTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new TimetablePage());
    }

    private async void OnUpdatesTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AnnouncementsPage());
    }

    private async void OnChildrenTapped(object sender, TappedEventArgs e)
    {
        // Already on My Children.
        await Task.CompletedTask;
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentProfilePage());
    }
}

// =============================================
// LEARNER MODEL
// =============================================

public class ChildItem : INotifyPropertyChanged
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = "Learner";
    public string Grade { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string AdmissionNumber { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string LearnerContactNumber { get; set; } = string.Empty;
    public string LearnerEmail { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ParentName { get; set; } = string.Empty;
    public string ParentContactNumber { get; set; } = string.Empty;
    public string ParentEmail { get; set; } = string.Empty;
    public string GuardianName { get; set; } = string.Empty;
    public string GuardianContactNumber { get; set; } = string.Empty;
    public string GuardianEmail { get; set; } = string.Empty;
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactNumber { get; set; } = string.Empty;
    public string MedicalInformation { get; set; } = string.Empty;
    public string Allergies { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";

    public string Initials
    {
        get
        {
            string[] parts = FullName.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return "?";

            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpperInvariant();

            return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        }
    }

    public string GradeDisplay =>
        string.IsNullOrWhiteSpace(Grade) ? "Grade not provided" : $"Grade {Grade}";

    public string ClassDisplay =>
        string.IsNullOrWhiteSpace(ClassName) ? "Class not provided" : $"Class: {ClassName}";

    public string AdmissionDisplay =>
        string.IsNullOrWhiteSpace(AdmissionNumber)
            ? "Admission no.: Not provided"
            : $"Admission no.: {AdmissionNumber}";

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
