
using System.Collections.ObjectModel;
using System.Globalization;

namespace KoosMatliApp.Views.Parent;

public partial class SchoolCalendarPage : ContentPage
{
    private readonly ObservableCollection<SchoolEventItem> _allEvents = new();

    public ObservableCollection<SchoolEventItem> FilteredEvents { get; } = new();

    private DateTime _displayedMonth;
    private DateTime _selectedDate;

    public SchoolCalendarPage()
    {
        InitializeComponent();
        BindingContext = this;

        _displayedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _selectedDate = DateTime.Today;

        LoadSampleEvents();
        RenderCalendar();
        ApplyEventFilter();
    }

    private void LoadSampleEvents()
    {
        // SAMPLE DATA ONLY.
        // Replace this list with events returned by your school API.

        _allEvents.Clear();

        _allEvents.Add(new SchoolEventItem
        {
            Id = "EVT001",
            Title = "Parent-Teacher Meeting",
            Description = "Meet teachers to discuss learner progress and academic goals.",
            Date = new DateTime(2026, 10, 30),
            StartTime = "14:00",
            EndTime = "16:00",
            Location = "School Hall",
            Category = "Important date"
        });

        _allEvents.Add(new SchoolEventItem
        {
            Id = "EVT002",
            Title = "School Sports Day",
            Description = "A day of sporting activities for learners and families.",
            Date = new DateTime(2026, 11, 7),
            StartTime = "08:00",
            EndTime = "13:00",
            Location = "School Sports Grounds",
            Category = "School event"
        });

        _allEvents.Add(new SchoolEventItem
        {
            Id = "EVT003",
            Title = "Assessment Week Begins",
            Description = "Learners begin their scheduled assessment activities.",
            Date = new DateTime(2026, 11, 16),
            StartTime = "08:00",
            EndTime = "15:00",
            Location = "Classrooms",
            Category = "Important date"
        });

        _allEvents.Add(new SchoolEventItem
        {
            Id = "EVT004",
            Title = "Year-End Concert",
            Description = "Learners showcase their performances and creative work.",
            Date = new DateTime(2026, 12, 4),
            StartTime = "17:00",
            EndTime = "19:00",
            Location = "School Hall",
            Category = "School event"
        });

        _allEvents.Add(new SchoolEventItem
        {
            Id = "EVT005",
            Title = "Public Holiday",
            Description = "School closure. Please check the official school calendar for term arrangements.",
            Date = new DateTime(2026, 12, 16),
            StartTime = "All day",
            EndTime = "",
            Location = "No classes",
            Category = "Important date"
        });
    }

    private void RenderCalendar()
    {
        MonthLabel.Text = _displayedMonth.ToString(
            "MMMM yyyy",
            CultureInfo.InvariantCulture);

        CalendarDaysGrid.Children.Clear();

        int daysInMonth = DateTime.DaysInMonth(
            _displayedMonth.Year,
            _displayedMonth.Month);

        // Sunday = column 0, Monday = column 1, etc.
        int firstDayOffset = (int)new DateTime(
            _displayedMonth.Year,
            _displayedMonth.Month,
            1).DayOfWeek;

        for (int day = 1; day <= daysInMonth; day++)
        {
            int position = firstDayOffset + day - 1;
            int row = position / 7;
            int column = position % 7;

            DateTime date = new DateTime(
                _displayedMonth.Year,
                _displayedMonth.Month,
                day);

            bool hasEvent = _allEvents.Any(item => item.Date.Date == date.Date);
            bool isSelected = date.Date == _selectedDate.Date;
            bool isToday = date.Date == DateTime.Today;

            var dayButton = new Button
            {
                Text = day.ToString(),
                FontSize = 13,
                Padding = 0,
                Margin = 1,
                CornerRadius = 10,
                BorderWidth = isToday && !isSelected ? 1 : 0,
                BorderColor = Color.FromArgb("#4338CA"),
                BackgroundColor = isSelected
                    ? Color.FromArgb("#243B80")
                    : Colors.Transparent,
                TextColor = isSelected
                    ? Colors.White
                    : Color.FromArgb("#334155"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                CommandParameter = date
            };

            dayButton.Clicked += OnCalendarDateClicked;

            CalendarDaysGrid.Children.Add(dayButton);
            Grid.SetRow(dayButton, row);
            Grid.SetColumn(dayButton, column);

            if (hasEvent)
            {
                var eventDot = new BoxView
                {
                    Color = Color.FromArgb("#F59E0B"),
                    WidthRequest = 5,
                    HeightRequest = 5,
                    CornerRadius = 3,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    InputTransparent = true
                };

                var cell = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = 5 }
                    },
                    InputTransparent = true
                };

                // Place the dot at the bottom of the date cell.
                cell.Children.Add(eventDot);
                Grid.SetRow(eventDot, 1);

                CalendarDaysGrid.Children.Add(cell);
                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, column);
            }
        }

        ApplyEventFilter();
    }

    private void OnCalendarDateClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not DateTime date)
        {
            return;
        }

        _selectedDate = date;
        RenderCalendar();
    }

    private void OnPreviousMonthClicked(object sender, EventArgs e)
    {
        _displayedMonth = _displayedMonth.AddMonths(-1);
        RenderCalendar();
    }

    private void OnNextMonthClicked(object sender, EventArgs e)
    {
        _displayedMonth = _displayedMonth.AddMonths(1);
        RenderCalendar();
    }

    private void ApplyEventFilter()
    {
        var matchingEvents = _allEvents
            .Where(item => item.Date.Date == _selectedDate.Date)
            .OrderBy(item => item.StartTime)
            .ToList();

        FilteredEvents.Clear();

        foreach (SchoolEventItem schoolEvent in matchingEvents)
        {
            FilteredEvents.Add(schoolEvent);
        }

        SelectedDateLabel.Text = _selectedDate.Date == DateTime.Today
            ? "Today's Events"
            : _selectedDate.ToString("dddd, dd MMMM", CultureInfo.InvariantCulture);

        EventCountLabel.Text = matchingEvents.Count == 1
            ? "1 event"
            : $"{matchingEvents.Count} events";

        StatusContainer.IsVisible = false;
        EventsCollectionView.IsVisible = true;
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

public class SchoolEventItem
{
    public string Id { get; set; } = "";

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime Date { get; set; }

    public string StartTime { get; set; } = "";

    public string EndTime { get; set; } = "";

    public string Location { get; set; } = "";

    public string Category { get; set; } = "School event";

    public string DayNumber => Date.Day.ToString();

    public string MonthShort => Date.ToString(
        "MMM",
        CultureInfo.InvariantCulture).ToUpperInvariant();

    public string TimeDisplay => string.IsNullOrWhiteSpace(EndTime)
        ? StartTime
        : $"{StartTime} – {EndTime}";

    public string LocationDisplay => $"Location: {Location}";
}
