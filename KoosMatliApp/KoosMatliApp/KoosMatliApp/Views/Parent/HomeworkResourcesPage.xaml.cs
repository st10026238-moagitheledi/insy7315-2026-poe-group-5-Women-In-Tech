using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KoosMatliApp.Views.Parent;

public partial class HomeworkResourcesPage : ContentPage
{
    private readonly ObservableCollection<LearningItem> _allItems = new();

    public ObservableCollection<LearningItem> FilteredItems { get; } = new();

    private string _selectedTab = "Homework";

    public HomeworkResourcesPage()
    {
        InitializeComponent();
        BindingContext = this;

        LoadSampleLearners();
        LoadSampleItems();
        UpdateSummary();
        ApplyFilters();
    }

    private void LoadSampleLearners()
    {
        // Sample learners for interface testing.
        // Replace these with children returned by ParentApiService.

        LearnerPicker.ItemsSource = new List<string>
        {
            "Lerato Mokoena",
            "Thabo Mokoena"
        };

        LearnerPicker.SelectedIndex = 0;
    }

    private void LoadSampleItems()
    {
        _allItems.Clear();

        // SAMPLE DATA ONLY.
        // Connect these items to your homework and resource API later.

        _allItems.Add(new LearningItem
        {
            Id = "HW001",
            LearnerName = "Lerato Mokoena",
            Title = "Fractions Worksheet",
            Subject = "Mathematics",
            Description = "Complete questions 1–10 on fractions and show all working.",
            Type = "Homework",
            DueDate = DateTime.Today.AddDays(2),
            Teacher = "Mr. Nkosi",
            Details = "Complete questions 1–10 in the fractions worksheet. Show your calculations and bring your completed work to class.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "HW002",
            LearnerName = "Lerato Mokoena",
            Title = "Reading Comprehension",
            Subject = "English",
            Description = "Read the assigned passage and answer the comprehension questions.",
            Type = "Homework",
            DueDate = DateTime.Today.AddDays(1),
            Teacher = "Ms. Dlamini",
            Details = "Read the assigned passage carefully and answer all comprehension questions in your exercise book.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "HW003",
            LearnerName = "Lerato Mokoena",
            Title = "Natural Sciences Project",
            Subject = "Natural Sciences",
            Description = "Prepare a short project about the water cycle.",
            Type = "Homework",
            DueDate = DateTime.Today.AddDays(5),
            Teacher = "Mrs. Molefe",
            Details = "Prepare a short project explaining the stages of the water cycle. Include a labelled diagram.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "RES001",
            LearnerName = "Lerato Mokoena",
            Title = "Fractions Study Guide",
            Subject = "Mathematics",
            Description = "A revision guide covering equivalent fractions and basic operations.",
            Type = "Resource",
            DueDate = null,
            Teacher = "Academic Resources",
            Details = "Use this guide to revise equivalent fractions, simplifying fractions, and addition and subtraction of fractions.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "RES002",
            LearnerName = "Lerato Mokoena",
            Title = "English Grammar Notes",
            Subject = "English",
            Description = "Revision notes covering sentence structure and parts of speech.",
            Type = "Resource",
            DueDate = null,
            Teacher = "Academic Resources",
            Details = "These notes cover nouns, verbs, adjectives, sentence structure and punctuation.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "HW004",
            LearnerName = "Thabo Mokoena",
            Title = "Number Patterns",
            Subject = "Numeracy",
            Description = "Complete the number patterns activity in your workbook.",
            Type = "Homework",
            DueDate = DateTime.Today.AddDays(2),
            Teacher = "Mr. Mokoena",
            Details = "Complete the number patterns activity in your workbook and check each answer.",
            ResourceUrl = ""
        });

        _allItems.Add(new LearningItem
        {
            Id = "RES003",
            LearnerName = "Thabo Mokoena",
            Title = "Reading Practice",
            Subject = "Reading",
            Description = "Short reading exercises to practise fluency and comprehension.",
            Type = "Resource",
            DueDate = null,
            Teacher = "Academic Resources",
            Details = "Practise reading the passages aloud and answer the comprehension questions after each passage.",
            ResourceUrl = ""
        });
    }

    private void UpdateSummary()
    {
        string learnerName = LearnerPicker.SelectedItem?.ToString() ?? "";

        var learnerItems = _allItems
            .Where(item => item.LearnerName.Equals(
                learnerName,
                StringComparison.OrdinalIgnoreCase))
            .ToList();

        HomeworkCountLabel.Text = learnerItems
            .Count(item => item.Type == "Homework")
            .ToString();

        ResourceCountLabel.Text = learnerItems
            .Count(item => item.Type == "Resource")
            .ToString();
    }

    private void ApplyFilters()
    {
        string learnerName = LearnerPicker.SelectedItem?.ToString() ?? "";
        string searchText = SearchBarControl?.Text?.Trim() ?? "";

        var results = _allItems.Where(item =>
        {
            bool matchesLearner = item.LearnerName.Equals(
                learnerName,
                StringComparison.OrdinalIgnoreCase);

            bool matchesTab = _selectedTab == "Homework"
                ? item.Type == "Homework"
                : item.Type == "Resource";

            bool matchesSearch =
                string.IsNullOrWhiteSpace(searchText) ||
                item.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Subject.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase);

            return matchesLearner && matchesTab && matchesSearch;
        })
        .OrderBy(item => item.DueDate ?? DateTime.MaxValue)
        .ToList();

        FilteredItems.Clear();

        foreach (LearningItem item in results)
        {
            FilteredItems.Add(item);
        }

        SectionTitleLabel.Text = _selectedTab == "Homework"
            ? "Homework Tasks"
            : "Learning Resources";

        LearningCollectionView.IsVisible = true;
        StatusContainer.IsVisible = false;
    }

    private void OnLearnerChanged(object sender, EventArgs e)
    {
        UpdateSummary();
        ApplyFilters();
    }

    private void OnTabClicked(object sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not string tab)
        {
            return;
        }

        _selectedTab = tab;

        HomeworkTabButton.BackgroundColor = tab == "Homework"
            ? Color.FromArgb("#243B80")
            : Colors.White;

        HomeworkTabButton.TextColor = tab == "Homework"
            ? Colors.White
            : Color.FromArgb("#334155");

        ResourcesTabButton.BackgroundColor = tab == "Resources"
            ? Color.FromArgb("#243B80")
            : Colors.White;

        ResourcesTabButton.TextColor = tab == "Resources"
            ? Colors.White
            : Color.FromArgb("#334155");

        ApplyFilters();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private async void OnItemTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not LearningItem item)
        {
            return;
        }

        string dateText = item.DueDate.HasValue
            ? $"Due date: {item.DueDate.Value:dd MMM yyyy}"
            : "No due date";

        string action = item.Type == "Homework"
            ? "Homework details"
            : "Resource details";

        await DisplayAlertAsync(
            item.Title,
            $"{action}\n\n" +
            $"Subject: {item.Subject}\n" +
            $"{dateText}\n" +
            $"Provided by: {item.Teacher}\n\n" +
            $"{item.Details}",
            "Close");
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ParentDashboardPage));
    }

    private async void OnScheduleTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimetablePage));
    }

    private async void OnUpdatesTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AnnouncementsPage));
    }

    private async void OnChildrenTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyChildrenPage));
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ParentProfilePage));
    }
}

public class LearningItem : INotifyPropertyChanged
{
    public string Id { get; set; } = "";

    public string LearnerName { get; set; } = "";

    public string Title { get; set; } = "";

    public string Subject { get; set; } = "";

    public string Description { get; set; } = "";

    public string Type { get; set; } = "Homework";

    public DateTime? DueDate { get; set; }

    public string Teacher { get; set; } = "";

    public string Details { get; set; } = "";

    public string ResourceUrl { get; set; } = "";

    public string TypeLabel => Type == "Homework"
        ? "HOMEWORK"
        : "RESOURCE";

    public string DateLabel => DueDate.HasValue
        ? $"Due: {DueDate.Value:dd MMM yyyy}"
        : "Learning material";

    public string TeacherLabel => $"Provided by {Teacher}";

    public string ActionLabel => Type == "Homework"
        ? "View task →"
        : "View resource →";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
