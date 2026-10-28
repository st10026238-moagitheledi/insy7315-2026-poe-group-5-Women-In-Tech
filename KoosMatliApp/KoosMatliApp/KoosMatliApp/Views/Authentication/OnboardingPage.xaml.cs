namespace KoosMatliApp.Views.Authentication;

public partial class OnboardingPage : ContentPage
{
    private int _currentSlide;

    private readonly string[] _titles =
    {
        "Stay Connected",
        "Timetables & School Events",
        "Access Information Offline",
        "Save Data, Stay Connected"
    };

    private readonly string[] _descriptions =
    {
        "Stay connected with your child's school through important announcements, messages and updates.",

        "Keep track of school timetables, upcoming events, important dates and school activities.",

        "Access previously saved school information even when you have limited or no internet connectivity.",

        "Use Data Saver Mode to reduce data usage while keeping up with important school information."
    };

    private readonly string[] _images =
    {
        "announcements.png",
        "timetables.png",
        "offline.png",
        "datasaver.png"
    };

    public OnboardingPage()
    {
        InitializeComponent();
        ShowSlide(0);
    }

    private void ShowSlide(int index)
    {
        _currentSlide = index;

        SlideNumber.Text = $"{index + 1:00} / 04";
        SlideTitle.Text = _titles[index];
        SlideDescription.Text = _descriptions[index];

        // Display the image for the current slide.
        SlideImage.Source = _images[index];

        // Highlight the current slide indicator.
        Dot1.WidthRequest = index == 0 ? 22 : 8;
        Dot2.WidthRequest = index == 1 ? 22 : 8;
        Dot3.WidthRequest = index == 2 ? 22 : 8;
        Dot4.WidthRequest = index == 3 ? 22 : 8;

        Dot1.Color = index == 0
            ? Color.FromArgb("#1239C9")
            : Color.FromArgb("#CBD5E1");

        Dot2.Color = index == 1
            ? Color.FromArgb("#1239C9")
            : Color.FromArgb("#CBD5E1");

        Dot3.Color = index == 2
            ? Color.FromArgb("#1239C9")
            : Color.FromArgb("#CBD5E1");

        Dot4.Color = index == 3
            ? Color.FromArgb("#1239C9")
            : Color.FromArgb("#CBD5E1");

        // Update navigation buttons.
        BackButton.IsVisible = index > 0;
        NextButton.Text = index == 3 ? "Get Started" : "Next";
        SkipButton.IsVisible = index < 3;
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        if (_currentSlide < 3)
        {
            ShowSlide(_currentSlide + 1);
        }
        else
        {
            _ = NavigateToLoginAsync();
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        if (_currentSlide > 0)
        {
            ShowSlide(_currentSlide - 1);
        }
    }

    private void OnSkipClicked(object sender, EventArgs e)
    {
        _ = NavigateToLoginAsync();
    }

    private async Task NavigateToLoginAsync()
    {
       await Navigation.PushAsync(new LoginPage());
    }
}
