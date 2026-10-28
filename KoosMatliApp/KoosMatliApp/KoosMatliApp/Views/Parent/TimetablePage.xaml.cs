
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KoosMatliApp.Views.Parent;

public partial class TimetablePage : ContentPage
{
    private readonly ObservableCollection<LessonItem> _allLessons = new();

    public ObservableCollection<LessonItem> FilteredLessons { get; } = new();

    private string _selectedDay = "Monday";

    public TimetablePage()
    {
        InitializeComponent();
        BindingContext = this;

        LoadSampleLearners();
        LoadSampleLessons();
        ApplyFilters();
    }

    private void LoadSampleLearners()
    {
        // Replace these sample learners with linked children
        // returned by your parent API.

        LearnerPicker.ItemsSource = new List<string>
        {
            "Lerato Mokoena",
            "Thabo Mokoena"
        };

        LearnerPicker.SelectedIndex = 0;
    }

    private void LoadSampleLessons()
    {
        _allLessons.Clear();

        // SAMPLE DATA ONLY — replace with the real timetable API.

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Monday",
            StartTime = "08:00",
            EndTime = "08:45",
            Subject = "Mathematics",
            Teacher = "Mr. Nkosi",
            Venue = "Room 12",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Monday",
            StartTime = "09:00",
            EndTime = "09:45",
            Subject = "English",
            Teacher = "Ms. Dlamini",
            Venue = "Room 8",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Monday",
            StartTime = "10:15",
            EndTime = "11:00",
            Subject = "Natural Sciences",
            Teacher = "Mrs. Molefe",
            Venue = "Science Lab",
            LessonType = "Practical"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Tuesday",
            StartTime = "08:00",
            EndTime = "08:45",
            Subject = "Life Skills",
            Teacher = "Ms. Maseko",
            Venue = "Room 5",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Wednesday",
            StartTime = "09:00",
            EndTime = "09:45",
            Subject = "Mathematics",
            Teacher = "Mr. Nkosi",
            Venue = "Room 12",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Thursday",
            StartTime = "08:00",
            EndTime = "08:45",
            Subject = "English",
            Teacher = "Ms. Dlamini",
            Venue = "Room 8",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Lerato Mokoena",
            Day = "Friday",
            StartTime = "09:00",
            EndTime = "09:45",
            Subject = "Social Sciences",
            Teacher = "Mr. Khumalo",
            Venue = "Room 7",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Thabo Mokoena",
            Day = "Monday",
            StartTime = "08:00",
            EndTime = "08:40",
            Subject = "Reading",
            Teacher = "Mrs. Zulu",
            Venue = "Room 3",
            LessonType = "Class"
        });

        _allLessons.Add(new LessonItem
        {
            LearnerName = "Thabo Mokoena",
            Day = "Tuesday",
            StartTime = "09:00",
            EndTime = "09:40",
            Subject = "Numeracy",
            Teacher = "Mr. Mokoena",
            Venue = "Room 4",
            LessonType = "Class"
        });
    }

    private void ApplyFilters()
    {
        string selectedLearner = LearnerPicker.SelectedItem?.ToString() ?? "";

        var lessons = _allLessons
            .Where(lesson =>
                lesson.Day.Equals(
                    _selectedDay,
                    StringComparison.OrdinalIgnoreCase) &&
                lesson.LearnerName.Equals(
                    selectedLearner,
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(lesson => lesson.StartTime)
            .ToList();

        FilteredLessons.Clear();

        foreach (LessonItem lesson in lessons)
        {
            FilteredLessons.Add(lesson);
        }

        SelectedDayLabel.Text = $"{_selectedDay}'s Lessons";
        LessonCountLabel.Text =
            $"{FilteredLessons.Count} lesson(s)";

        StatusContainer.IsVisible = false;
        TimetableCollectionView.IsVisible = true;
    }

    private void OnLearnerChanged(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    private void OnDayClicked(object sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not string day)
        {
            return;
        }

        _selectedDay = day;

        ResetDayButtons();

        button.BackgroundColor = Color.FromArgb("#243B80");
        button.TextColor = Colors.White;

        ApplyFilters();
    }

    private void ResetDayButtons()
    {
        Button[] buttons =
        {
            MondayButton,
            TuesdayButton,
            WednesdayButton,
            ThursdayButton,
            FridayButton
        };

        foreach (Button button in buttons)
        {
            button.BackgroundColor = Colors.White;
            button.TextColor = Color.FromArgb("#334155");
        }
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ParentDashboardPage));
    }

    private async void OnScheduleTapped(object sender, TappedEventArgs e)
    {
        // Already on the Timetable page.
        await Task.CompletedTask;
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

public class LessonItem : INotifyPropertyChanged
{
    public string LearnerName { get; set; } = "";

    public string Day { get; set; } = "";

    public string StartTime { get; set; } = "";

    public string EndTime { get; set; } = "";

    public string Subject { get; set; } = "";

    public string Teacher { get; set; } = "";

    public string Venue { get; set; } = "";

    public string LessonType { get; set; } = "Class";

    public string TeacherDisplay => $"Teacher: {Teacher}";

    public string VenueDisplay => $"Venue: {Venue}";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
