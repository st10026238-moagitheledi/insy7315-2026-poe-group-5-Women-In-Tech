using KoosMatliApp.Localization;
using KoosMatliApp.Views.Admin;
using Microsoft.Maui.Controls.Shapes;

namespace KoosMatliApp;

public partial class AdminShell : Shell
{
    private static bool _routesRegistered;
    private bool _navigating;

    private static readonly
        System.Runtime.CompilerServices.ConditionalWeakTable<Page, Label>
        NavigationErrors = new();

    private static readonly Color Navy = Color.FromArgb("#102D53");
    private static readonly Color Gold = Color.FromArgb("#FFBF00");

    public AdminShell()
    {
        Title = "Koos Matli Primary School";
        Resources.MergedDictionaries.Add(new AdminThemeResources.AdminBrandStyles());
        FlyoutBehavior = FlyoutBehavior.Disabled;

        if (!_routesRegistered)
        {
            Register<Schooldetailspage>("schooldetails");
            Register<SchoolDatesPage>("schooldates");
            Register<Schoolcalendarpage>("schoolcalendar");
            Register<Documentspage>("documents");
            Register<Applicationsdetailspage>("applicationdetail");
            Register<AddCalendarEventPage>("addcalendarevent");
            Register<AddNewUserPage>("addnewuser");

            Register<AccountSettingsPage>("settings-account");
            Register<RolesPermissionsPage>("settings-roles");
            Register<SecuritySettingsPage>("settings-security");
            Register<BackupRestorePage>("settings-backup");
            Register<AuditLogsPage>("settings-audit");
            Register<SystemSettingsPage>("settings-system");

            _routesRegistered = true;
        }

        var bar = new TabBar { Route = "main" };

        AddTab<AdminDashboardPage>(bar, "Home", "dashboard");
        AddTab<UserPage>(bar, "Users", "users");
        AddTab<ApplicationsPage>(bar, "Apps", "applications");
        AddTab<StaffPage>(bar, "Staff", "staff");
        AddTab<ProfilePage>(bar, "Settings", "profile");

        Items.Add(bar);
    }

    private static void Register<T>(string route)
        where T : ContentPage, new()
    {
        Routing.RegisterRoute(
            route,
            new PageFactory(() => CreatePage<T>()));
    }

    private static void AddTab<T>(
        TabBar bar, string title, string route)
        where T : ContentPage, new()
    {
        var tab = new Tab
        {
            Title = L.T(title),
            Route = "section-" + route
        };

        tab.Items.Add(new ShellContent
        {
            Title = L.T(title),
            Route = route,
            ContentTemplate = new DataTemplate(() => CreatePage<T>())
        });

        bar.Items.Add(tab);
    }

    private sealed class PageFactory(
        Func<ContentPage> create) : RouteFactory
    {
        public override Element GetOrCreate() => create();

        public override Element GetOrCreate(
            IServiceProvider services) => create();
    }

    private static T CreatePage<T>() where T : ContentPage, new()
    {
        var page = new T();

        Shell.SetTabBarIsVisible(page, false);
        Shell.SetNavBarIsVisible(page, false);

        page.SafeAreaEdges = new Microsoft.Maui.SafeAreaEdges(
            Microsoft.Maui.SafeAreaRegions.Container);

        if (page is ProfilePage)
        {
            page.SetAppThemeColor(
                BackgroundColorProperty,
                Color.FromArgb("#FFE7A8"),
                Color.FromArgb("#1D2531"));
        }

        if (page is SettingsPage &&
            page.Content is ScrollView scroll &&
            scroll.Content is VerticalStackLayout body &&
            body.Children.FirstOrDefault() is Button oldBack &&
            oldBack.Text == L.T("← Back"))
        {
            body.Children.Remove(oldBack);
        }

        var original = page.Content;
        page.Content = null;

        var layout = new Grid
        {
            BindingContext = page.BindingContext,
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            }
        };

        page.BindingContextChanged += (_, _) =>
            layout.BindingContext = page.BindingContext;

        if (page is not AdminDashboardPage)
        {
            var header = new HorizontalStackLayout
            {
                Spacing = 12,
                Padding = new Thickness(16, 8)
            };

            var back = new Button
            {
                Text = L.T("← Back"),
                MinimumHeightRequest = 44
            };

            back.Clicked += async (_, _) =>
                await NavigateAsync(BackRoute(page));

            header.Add(back);

            if (page is SettingsPage)
            {
                header.Add(new Label
                {
                    Text = page.Title,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center
                });
            }

            layout.Add(header, 0, 0);
        }

        var navigationError = new Label
        {
            TextColor = Colors.Red,
            IsVisible = false,
            Margin = new Thickness(16, 4)
        };

        NavigationErrors.Add(page, navigationError);
        layout.Add(navigationError, 0, 1);

        if (original is not null)
            layout.Add(original, 0, 2);

        layout.Add(CreateBottomBar(page), 0, 3);

        page.Content = layout;
        return page;
    }

    private static Grid CreateBottomBar(Page page)
    {
        var tabs = new Grid
        {
            ColumnSpacing = 0,
            BackgroundColor = Navy,
            Padding = new Thickness(0, 4)
        };

        var destinations = new[]
        {
            ("Home", "dashboard"),
            ("Users", "users"),
            ("Apps", "applications"),
            ("Staff", "staff"),
            ("Settings", "profile")
        };

        string selected = Section(page);

        for (int i = 0; i < destinations.Length; i++)
        {
            tabs.ColumnDefinitions.Add(
                new ColumnDefinition(GridLength.Star));

            var (title, route) = destinations[i];
            var color = selected == route ? Gold : Colors.White;

            var cell = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition(26),
                    new RowDefinition(GridLength.Auto)
                },
                Padding = new Thickness(2, 4),
                MinimumHeightRequest = 56
            };

            cell.Add(new Microsoft.Maui.Controls.Shapes.Path
            {
                Data = (Geometry)new PathGeometryConverter()
                    .ConvertFromInvariantString(IconPath(route))!,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 1.8,
                Fill = Brush.Transparent,
                WidthRequest = 26,
                HeightRequest = 26,
                HorizontalOptions = LayoutOptions.Center,
                InputTransparent = true
            }, 0, 0);

            cell.Add(new Label
            {
                Text = L.T(title),
                TextColor = color,
                FontSize = 11,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                InputTransparent = true
            }, 0, 1);

            var button = new Button
            {
                Text = "",
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0,
                Padding = 0,
                MinimumWidthRequest = 0,
                MinimumHeightRequest = 56
            };

            SemanticProperties.SetDescription(button, L.T(title));

            button.Clicked += async (_, _) =>
                await NavigateAsync("//" + route);

            Grid.SetRowSpan(button, 2);
            cell.Add(button, 0, 0);
            tabs.Add(cell, i, 0);
        }

        return tabs;
    }

    private static string IconPath(string route) => route switch
    {
        "dashboard" =>
            "M2,12 L13,3 L24,12 M5,12 L5,23 L21,23 L21,12 " +
            "M11,23 L11,16 L15,16 L15,23",

        "users" =>
            "M10,7 A3,3 0 1 0 16,7 A3,3 0 1 0 10,7 " +
            "M8,23 L8,17 Q8,13 13,13 Q18,13 18,17 L18,23 Z " +
            "M2,9 A2,2 0 1 0 6,9 A2,2 0 1 0 2,9 " +
            "M1,22 L1,17 Q1,14 6,14 " +
            "M20,9 A2,2 0 1 0 24,9 A2,2 0 1 0 20,9 " +
            "M25,22 L25,17 Q25,14 20,14",

        "applications" =>
            "M2,6 L11,6 L14,9 L24,9 L24,22 L2,22 Z M2,12 L24,12",

        "staff" =>
            "M4,7 A3,3 0 1 0 10,7 A3,3 0 1 0 4,7 " +
            "M2,23 L2,17 Q2,13 7,13 Q12,13 12,17 L12,23 Z " +
            "M15,4 L24,4 L24,21 L15,21 Z " +
            "M17,8 L22,8 M17,12 L22,12 M17,16 L22,16",

        _ =>
            "M9,7 A4,4 0 1 0 17,7 A4,4 0 1 0 9,7 " +
            "M5,24 L5,19 Q5,13 13,13 Q21,13 21,19 L21,24 Z"
    };

    private static string Section(Page page) => page switch
    {
        UserPage or AddNewUserPage => "users",
        ApplicationsPage or Applicationsdetailspage => "applications",
        StaffPage => "staff",
        ProfilePage or SettingsPage => "profile",
        _ => "dashboard"
    };

    private static string BackRoute(Page page) => page switch
    {
        SettingsPage => "//profile",
        Applicationsdetailspage => "//applications",
        AddNewUserPage => "//users",
        AddCalendarEventPage => "..",
        _ => "//dashboard"
    };

    private static async Task NavigateAsync(string route)
    {
        if (Shell.Current is not AdminShell shell || shell._navigating)
            return;

        shell._navigating = true;

        try
        {
            if (shell.CurrentPage is { } current &&
                NavigationErrors.TryGetValue(current, out var previous))
            {
                previous.Text = "";
                previous.IsVisible = false;
            }

            await shell.GoToAsync(route);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());

            // Avoid awaiting a modal dialog after native navigation fails.
            try
            {
                if (shell.CurrentPage is { } current &&
                    NavigationErrors.TryGetValue(current, out var error))
                {
                    error.Text = L.T("Error") + ": " + ex.Message;
                    error.IsVisible = true;
                }
            }
            catch (Exception displayError)
            {
                System.Diagnostics.Debug.WriteLine(displayError.ToString());
            }
        }
        finally
        {
            shell._navigating = false;
        }
    }

    public static Task GoHomeAsync() =>
        Shell.Current?.GoToAsync("//dashboard")
        ?? Task.CompletedTask;

    public static Task GoBackAsync() =>
        Shell.Current is { } shell
            ? shell.GoToAsync(BackRoute(shell.CurrentPage))
            : Task.CompletedTask;
}