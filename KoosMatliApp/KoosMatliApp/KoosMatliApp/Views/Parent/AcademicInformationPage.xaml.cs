namespace KoosMatliApp.Views.Parent;

public partial class AcademicInformationPage : ContentPage
{
    public AcademicInformationPage()
    {
        InitializeComponent();

        AcademicYearLabel.Text = DateTime.Today.Year.ToString();

        LoadSampleLearners();
    }

    private void LoadSampleLearners()
    {
        // Sample data for UI testing.
        // Replace with the real linked children returned by your API.

        LearnerPicker.ItemsSource = new List<LearnerAcademicProfile>
        {
            new()
            {
                Id = "LEARNER001",
                FullName = "Lerato Mokoena",
                Grade = "Grade 5A"
            },
            new()
            {
                Id = "LEARNER002",
                FullName = "Thabo Mokoena",
                Grade = "Grade 3B"
            }
        };

        LearnerPicker.ItemDisplayBinding =
            new Binding(nameof(LearnerAcademicProfile.FullName));

        LearnerPicker.SelectedIndex = 0;
    }

    private void OnLearnerChanged(object sender, EventArgs e)
    {
        if (LearnerPicker.SelectedItem is not LearnerAcademicProfile learner)
        {
            return;
        }

        LearnerNameLabel.Text = learner.FullName;
        GradeLabel.Text = string.IsNullOrWhiteSpace(learner.Grade)
            ? "Not specified"
            : learner.Grade;

        StatusContainer.IsVisible = false;
    }

    private async void OnAssessmentsTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlertAsync(
            "Assessment Results",
            "Assessment results will appear here once connected to the school's academic records API.",
            "OK");
    }

    private async void OnAttendanceTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlertAsync(
            "Attendance Record",
            "Attendance information will appear here once connected to the school's attendance API.",
            "OK");
    }

    private async void OnFeedbackTapped(object sender, TappedEventArgs e)
    {
        await DisplayAlertAsync(
            "Teacher Feedback",
            "Teacher comments and academic feedback will appear here once provided by the school API.",
            "OK");
    }

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
        await Navigation.PushAsync(new MyChildrenPage());
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentProfilePage());
    }
}

public class LearnerAcademicProfile
{
    public string Id { get; set; } = "";

    public string FullName { get; set; } = "";

    public string Grade { get; set; } = "";
}
