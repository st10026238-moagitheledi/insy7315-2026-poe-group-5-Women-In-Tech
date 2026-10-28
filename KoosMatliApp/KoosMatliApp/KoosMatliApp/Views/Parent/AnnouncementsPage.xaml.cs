using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KoosMatliApp.Views.Parent;

public partial class AnnouncementsPage : ContentPage
{
    private readonly ObservableCollection<AnnouncementItem> _allAnnouncements = new();

    public ObservableCollection<AnnouncementItem> FilteredAnnouncements { get; } = new();

    private string _selectedCategory = "All";

    public AnnouncementsPage()
    {
        InitializeComponent();
        BindingContext = this;

        LoadSampleAnnouncements();
        ApplyFilters();
    }

    private void LoadSampleAnnouncements()
    {
        _allAnnouncements.Clear();

        // Sample data for UI testing.
        // Replace this list with data from your announcements API.

        _allAnnouncements.Add(new AnnouncementItem
        {
            Id = "1",
            Title = "Important Academic Notice",
            Category = "Academic",
            Summary = "Please check your academic portal for assessment dates, submission deadlines and important course updates.",
            Details = "Students and parents are encouraged to check the academic portal regularly for assessment dates, submission deadlines and course updates. Contact the school office if you need assistance.",
            DatePublished = DateTime.Today,
            Author = "Academic Office"
        });

        _allAnnouncements.Add(new AnnouncementItem
        {
            Id = "2",
            Title = "Upcoming School Event",
            Category = "Events",
            Summary = "Keep an eye on the school calendar for upcoming activities, meetings and school events.",
            Details = "Please refer to the official school calendar for confirmed dates, times and venues for upcoming activities. Further details will be shared through the school communication channels.",
            DatePublished = DateTime.Today.AddDays(-1),
            Author = "School Administration"
        });

        _allAnnouncements.Add(new AnnouncementItem
        {
            Id = "3",
            Title = "Parent Communication Reminder",
            Category = "General",
            Summary = "Please ensure that your contact details are up to date so you can receive school notifications.",
            Details = "Parents and guardians should ensure that their phone numbers and email addresses are current. Updated contact details help the school communicate important information efficiently.",
            DatePublished = DateTime.Today.AddDays(-3),
            Author = "School Office"
        });
    }

    private void ApplyFilters()
    {
        string searchText = AnnouncementSearchBar?.Text?.Trim() ?? "";

        var results = _allAnnouncements.Where(item =>
        {
            bool matchesCategory =
                _selectedCategory == "All" ||
                string.Equals(
                    item.Category,
                    _selectedCategory,
                    StringComparison.OrdinalIgnoreCase);

            bool matchesSearch =
                string.IsNullOrWhiteSpace(searchText) ||
                item.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Summary.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Category.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                item.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase);

            return matchesCategory && matchesSearch;
        }).OrderByDescending(item => item.DatePublished).ToList();

        FilteredAnnouncements.Clear();

        foreach (var announcement in results)
        {
            FilteredAnnouncements.Add(announcement);
        }

        AnnouncementCountLabel.Text =
            $"{FilteredAnnouncements.Count} announcement(s)";

        AnnouncementsCollectionView.IsVisible = true;
        StatusContainer.IsVisible = false;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnCategoryClicked(object sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not string category)
        {
            return;
        }

        _selectedCategory = category;

        ResetCategoryButtons();

        button.BackgroundColor = Color.FromArgb("#243B80");
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
            button.TextColor = Color.FromArgb("#334155");
        }
    }

    private async void OnAnnouncementTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not AnnouncementItem announcement)
        {
            return;
        }

        await DisplayAlertAsync(
            announcement.Title,
            $"{announcement.Category} · {announcement.DateDisplay}\n\n" +
            $"{announcement.Details}\n\n" +
            $"Posted by: {announcement.Author}",
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
        // Already on the Announcements page.
        await Task.CompletedTask;
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

public class AnnouncementItem : INotifyPropertyChanged
{
    public string Id { get; set; } = "";

    public string Title { get; set; } = "";

    public string Category { get; set; } = "General";

    public string Summary { get; set; } = "";

    public string Details { get; set; } = "";

    public DateTime DatePublished { get; set; }

    public string Author { get; set; } = "School Administration";

    public string DateDisplay => DatePublished.ToString("dd MMM yyyy");

    public string AuthorDisplay => $"Posted by {Author}";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}