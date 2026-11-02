using KoosMatliApp.Features.Teacher.Views;

namespace KoosMatliApp.Features.Teacher;

public partial class TeacherShell : Shell
{
    private static readonly Color Navy =
        Color.FromArgb("#102D53");

    private static readonly Color Gold =
        Color.FromArgb("#FFBF00");

    private static readonly string[] Titles =
    {
        "Home",
        "Classes",
        "Updates",
        "Schedule",
        "Profile"
    };

    private static readonly string[] Routes =
    {
        "TeacherHome",
        "TeacherClasses",
        "TeacherUpdates",
        "TeacherSchedule",
        "TeacherProfile"
    };

    private static readonly string[] Icons =
    {
        "nav_home.png",
        "nav_classes.png",
        "nav_updates.png",
        "nav_schedule.png",
        "nav_profile.png"
    };

    private static bool _routesRegistered;
    private bool _isNavigating;

    public TeacherShell()
    {
        InitializeComponent();

        RegisterRoutes();

        var tabs = new TabBar
        {
            Route = "TeacherTabs"
        };

        AddTab<TeacherHomePage>(tabs, 0);
        AddTab<ClassesPage>(tabs, 1);
        AddTab<UpdatesPage>(tabs, 2);
        AddTab<SchedulePage>(tabs, 3);
        AddTab<ProfilePage>(tabs, 4);

        Items.Add(tabs);
    }

    private void AddTab<TPage>(
        TabBar tabs,
        int index)
        where TPage : ContentPage, new()
    {
        var tab = new Tab
        {
            Title = Titles[index],
            Route = $"{Routes[index]}Section"
        };

        tab.Items.Add(new ShellContent
        {
            Title = Titles[index],
            Route = Routes[index],

            ContentTemplate = new DataTemplate(
                () => CreatePage<TPage>(index))
        });

        tabs.Items.Add(tab);
    }

    private ContentPage CreatePage<TPage>(
        int selectedIndex)
        where TPage : ContentPage, new()
    {
        var page = new TPage();

        Shell.SetNavBarIsVisible(page, false);
        Shell.SetTabBarIsVisible(page, false);

        // Keep the existing page content and its bindings.
        var originalContent = page.Content;
        page.Content = null;

        var layout = new Grid
        {
            RowSpacing = 0,

            RowDefinitions =
            {
                new RowDefinition
                {
                    Height = GridLength.Star
                },
                new RowDefinition
                {
                    Height = GridLength.Auto
                }
            }
        };

        if (originalContent != null)
        {
            Grid.SetRow(originalContent, 0);
            layout.Children.Add(originalContent);
        }

        var bottomBar = CreateBottomBar(selectedIndex);

        Grid.SetRow(bottomBar, 1);
        layout.Children.Add(bottomBar);

        page.Content = layout;

        return page;
    }

    private Grid CreateBottomBar(
        int selectedIndex)
    {
        var bar = new Grid
        {
            BackgroundColor = Navy,
            Padding = new Thickness(4, 8),
            ColumnSpacing = 0
        };

        for (var index = 0; index < Titles.Length; index++)
        {
            bar.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = GridLength.Star
                });

            var isSelected = index == selectedIndex;

            var content = new VerticalStackLayout
            {
                Spacing = 4,
                InputTransparent = true,

                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                Children =
                {
                    new Image
                    {
                        Source = Icons[index],
                        WidthRequest = 24,
                        HeightRequest = 24,
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.Center
                    },

                    new Label
                    {
                        Text = Titles[index],
                        FontSize = 11,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = isSelected
                            ? Gold
                            : Colors.White,

                        HorizontalTextAlignment =
                            TextAlignment.Center
                    }
                }
            };

            var cell = new Grid
            {
                MinimumHeightRequest = 58,

                RowDefinitions =
                {
                    new RowDefinition
                    {
                        Height = 3
                    },
                    new RowDefinition
                    {
                        Height = GridLength.Star
                    }
                }
            };

            var selectionIndicator = new BoxView
            {
                Color = isSelected
                    ? Gold
                    : Colors.Transparent,

                WidthRequest = 36,
                HorizontalOptions = LayoutOptions.Center
            };

            Grid.SetRow(selectionIndicator, 0);
            cell.Children.Add(selectionIndicator);

            Grid.SetRow(content, 1);
            cell.Children.Add(content);

            var button = new Button
            {
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0,
                CornerRadius = 0,
                Padding = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            SemanticProperties.SetDescription(
                button,
                Titles[index]);

            var destinationIndex = index;

            button.Clicked += async (_, _) =>
                await NavigateAsync(
                    selectedIndex,
                    destinationIndex);

            Grid.SetRowSpan(button, 2);
            cell.Children.Add(button);

            Grid.SetColumn(cell, index);
            bar.Children.Add(cell);
        }

        return bar;
    }

    private async Task NavigateAsync(
        int selectedIndex,
        int destinationIndex)
    {
        if (_isNavigating ||
            selectedIndex == destinationIndex)
        {
            return;
        }

        try
        {
            _isNavigating = true;

            await GoToAsync(
                $"//TeacherTabs/" +
                $"{Routes[destinationIndex]}Section/" +
                Routes[destinationIndex]);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Teacher navigation error: {ex}");

            if (CurrentPage is { } page)
            {
                await page.DisplayAlertAsync(
                    "Navigation",
                    "Unable to open this page. Please try again.",
                    "OK");
            }
        }
        finally
        {
            _isNavigating = false;
        }
    }

    private static void RegisterRoutes()
    {
        if (_routesRegistered)
        {
            return;
        }

        Routing.RegisterRoute(
            nameof(ClassDetailsPage),
            typeof(ClassDetailsPage));

        Routing.RegisterRoute(
            nameof(HomeworkPage),
            typeof(HomeworkPage)); // (Microsoft, 2026)

        Routing.RegisterRoute(
            nameof(AddHomeworkPage),
            typeof(AddHomeworkPage));

        Routing.RegisterRoute(
            nameof(AddLearnerPage),
            typeof(AddLearnerPage));

        Routing.RegisterRoute(
            nameof(LearnerDetailsPage),
            typeof(LearnerDetailsPage));

        Routing.RegisterRoute(
            nameof(AddAnnouncementPage),
            typeof(AddAnnouncementPage));

        Routing.RegisterRoute(
            nameof(AnnouncementDetailsPage),
            typeof(AnnouncementDetailsPage));

        Routing.RegisterRoute(
            nameof(AddSchedulePage),
            typeof(AddSchedulePage));

        Routing.RegisterRoute(
            nameof(ScheduleDetailsPage),
            typeof(ScheduleDetailsPage));

        Routing.RegisterRoute(
            nameof(PersonalInformationPage),
            typeof(PersonalInformationPage));

        Routing.RegisterRoute(
            nameof(ChangePasswordPage),
            typeof(ChangePasswordPage));

        Routing.RegisterRoute(
            nameof(LanguageSettingsPage),
            typeof(LanguageSettingsPage));

        Routing.RegisterRoute(
            nameof(NotificationSettingsPage),
            typeof(NotificationSettingsPage));

        Routing.RegisterRoute(
            nameof(HelpSupportPage),
            typeof(HelpSupportPage));

        Routing.RegisterRoute(
            nameof(AboutAppPage),
            typeof(AboutAppPage)); // (Microsoft, 2026)

        _routesRegistered = true;
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].