using System.Text.Json;
using KoosMatliApp.Services;

namespace KoosMatliApp.Views.Parent;

public partial class ParentDashboardPage : ContentPage
{
    private readonly ParentApiService _parentApiService;
    private bool _isLoading;

    public ParentDashboardPage()
    {
        InitializeComponent();

        _parentApiService =
            Application.Current!.Handler!.MauiContext!
                .Services.GetRequiredService<ParentApiService>();

        Loaded += async (_, _) => await LoadDashboardAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        if (_isLoading)
            return;

        _isLoading = true;
        DashboardRefreshView.IsRefreshing = true;
        LoadingOverlay.IsVisible = true;
        StatusLabel.Text = "Loading your school information...";

        try
        {
            using JsonDocument document =
                await _parentApiService.GetDashboardAsync();

            JsonElement root = document.RootElement;

            if (root.TryGetProperty("success", out JsonElement success)
                && success.ValueKind == JsonValueKind.False)
            {
                throw new HttpRequestException(
                    ReadString(root, "message") ??
                    "Unable to load your dashboard.");
            }

            JsonElement parent = GetProperty(root, "parent");

            string parentName =
                ReadString(parent, "displayName") ??
                ReadString(parent, "name") ??
                "Parent";

            WelcomeLabel.Text = $"Welcome, {parentName}!";

            RenderChildren(GetArray(root, "children"));
            RenderAnnouncements(GetArray(root, "announcements"));
            RenderEvents(GetArray(root, "upcomingEvents"));

            StatusLabel.Text = "Your school information is up to date.";
        }
        catch (UnauthorizedAccessException ex)
        {
            StatusLabel.Text = ex.Message;

            await DisplayAlertAsync(
                "Session expired",
                "Please log in again to access your dashboard.",
                "OK");

            // We will connect this to your existing login flow next.
        }
        catch (HttpRequestException ex)
        {
            StatusLabel.Text = ex.Message;
        }
        catch (TaskCanceledException)
        {
            StatusLabel.Text =
                "The request timed out. Please try again.";
        }
        catch (JsonException)
        {
            StatusLabel.Text =
                "The server returned data in an unexpected format.";
        }
        catch (Exception ex)
        {
            StatusLabel.Text =
                $"Unable to load your dashboard: {ex.Message}";
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
            DashboardRefreshView.IsRefreshing = false;
            _isLoading = false;
        }
    }

    private void RenderChildren(JsonElement children)
    {
        ChildrenContainer.Clear();

        if (children.ValueKind != JsonValueKind.Array ||
            children.GetArrayLength() == 0)
        {
            ChildrenContainer.Add(CreateCard(
                "No linked children found",
                "If you expect to see a child here, contact the school."));
            return;
        }

        foreach (JsonElement child in children.EnumerateArray())
        {
            string name =
                ReadString(child, "fullName") ??
                ReadString(child, "name") ??
                ReadString(child, "displayName") ??
                "Learner";

            string details = string.Join(
                " • ",
                new[]
                {
                    ReadString(child, "grade"),
                    ReadString(child, "className")
                }.Where(value => !string.IsNullOrWhiteSpace(value)));

            ChildrenContainer.Add(CreateCard(
                name,
                string.IsNullOrWhiteSpace(details)
                    ? "Linked learner"
                    : details));
        }
    }

    private void RenderAnnouncements(JsonElement announcements)
    {
        AnnouncementsContainer.Clear();

        if (announcements.ValueKind != JsonValueKind.Array ||
            announcements.GetArrayLength() == 0)
        {
            AnnouncementsContainer.Add(CreateCard(
                "No announcements",
                "There are no published announcements to display."));
            return;
        }

        foreach (JsonElement announcement in
                 announcements.EnumerateArray())
        {
            string title =
                ReadString(announcement, "title") ??
                "School announcement";

            string message =
                ReadString(announcement, "message") ??
                ReadString(announcement, "description") ??
                ReadString(announcement, "content") ??
                "Open the school announcement for more information.";

            AnnouncementsContainer.Add(CreateCard(title, message));
        }
    }

    private void RenderEvents(JsonElement events)
    {
        EventsContainer.Clear();

        if (events.ValueKind != JsonValueKind.Array ||
            events.GetArrayLength() == 0)
        {
            EventsContainer.Add(CreateCard(
                "No upcoming events",
                "There are currently no upcoming events."));
            return;
        }

        foreach (JsonElement schoolEvent in events.EnumerateArray())
        {
            string title =
                ReadString(schoolEvent, "title") ??
                ReadString(schoolEvent, "name") ??
                "School event";

            string date =
                ReadString(schoolEvent, "date") ??
                ReadString(schoolEvent, "startDate") ??
                "Date to be confirmed";

            EventsContainer.Add(CreateCard(title, date));
        }
    }


    private static View CreateCard(string title, string description)
    {
        var titleLabel = new Label
        {
            Text = title,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#061D45")
        };

        var descriptionLabel = new Label
        {
            Text = description,
            FontSize = 13,
            TextColor = Color.FromArgb("#52627A")
        };

        var content = new VerticalStackLayout
        {
            Spacing = 6,
            Children = { titleLabel, descriptionLabel }
        };

        return new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#E0E7F1"),
            StrokeThickness = 1,
            Padding = new Thickness(15),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle
            {
                CornerRadius = new CornerRadius(14)
            },
            Content = content
        };
    }


    private static JsonElement GetProperty(
        JsonElement element,
        string propertyName)
    {
        return element.ValueKind == JsonValueKind.Object &&
               element.TryGetProperty(propertyName, out JsonElement value)
            ? value
            : default;
    }

    private static JsonElement GetArray(
        JsonElement element,
        string propertyName)
    {
        JsonElement value = GetProperty(element, propertyName);

        return value.ValueKind == JsonValueKind.Array
            ? value
            : default;
    }

    private static string? ReadString(
        JsonElement element,
        string propertyName)
    {
        JsonElement value = GetProperty(element, propertyName);

        if (value.ValueKind == JsonValueKind.String)
            return value.GetString();

        if (value.ValueKind == JsonValueKind.Number)
            return value.ToString();

        return null;
    }
}
