using KoosMatliApp.Models;
using KoosMatliApp.Services;
using Microsoft.Maui.Controls.Shapes;

namespace KoosMatliApp.Views.Admin;

public partial class ApplicationsPage : ContentPage
{
    private readonly IAdminApiService _api;
    private readonly List<AdmissionApplication> _applications = new();
    private readonly List<Button> _actions = new();

    private bool _busy;
    private bool _ready;
    private int _filterIndex;

    public ApplicationsPage()
    {
        InitializeComponent();

        _api = App.Services!.GetRequiredService<IAdminApiService>();

        RefreshButton.Text = L.T("Refresh");
        _ready = true;

        UpdateFilterButtons();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RunAsync(LoadAsync);
    }

    private Button[] FilterButtons =>
        [AllFilter, PendingFilter, ApprovedFilter, RejectedFilter];

    private async Task LoadAsync()
    {
        if (!App.Services!.GetRequiredService<AdminSession>().IsConnected)
        {
            Feedback.Text =
                L.T("Connect the signed-in user before loading admin data.");
            return;
        }

        var records = await _api.GetApplicationsAsync();

        _applications.Clear();
        _applications.AddRange(records);

        Render();
    }

    private async Task RunAsync(Func<Task> action)
    {
        if (_busy)
            return;

        _busy = true;
        Feedback.Text = "";
        SetBusy(true);

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            Feedback.Text = ex.Message;
        }
        finally
        {
            _busy = false;
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        RefreshButton.IsEnabled = !busy;
        SearchInput.IsEnabled = !busy;

        foreach (var button in FilterButtons)
            button.IsEnabled = !busy;

        Loading.IsVisible = busy;
        Loading.IsRunning = busy;

        foreach (var button in _actions)
            button.IsEnabled = !busy;
    }

    private void SearchChanged(object sender, TextChangedEventArgs e)
    {
        if (_ready && !_busy)
            Render();
    }

    private void StatusClicked(object sender, EventArgs e)
    {
        if (!_ready || _busy ||
            sender is not Button button ||
            !int.TryParse(
                button.CommandParameter?.ToString(), out var index))
        {
            return;
        }

        _filterIndex = index;
        UpdateFilterButtons();
        Render();
    }

    private void UpdateFilterButtons()
    {
        var buttons = FilterButtons;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].BackgroundColor = Color.FromArgb(
                i == _filterIndex ? "#FFBF00" : "#102D53");

            buttons[i].TextColor = i == _filterIndex
                ? Color.FromArgb("#102D53")
                : Colors.White;
        }
    }

    private async void RefreshClicked(object sender, EventArgs e) =>
        await RunAsync(LoadAsync);

    private void Render()
    {
        ApplicationCards.Children.Clear();
        _actions.Clear();

        string search = SearchInput.Text?.Trim() ?? "";

        var filtered = _applications.Where(application =>
            (_filterIndex switch
            {
                1 => CanDecide(application),
                2 => string.Equals(
                    application.Status, "Approved",
                    StringComparison.OrdinalIgnoreCase),
                3 => string.Equals(
                    application.Status, "Rejected",
                    StringComparison.OrdinalIgnoreCase),
                _ => true
            }) &&
            (search.Length == 0 ||
             application.ApplicationNumber.Contains(
                 search, StringComparison.OrdinalIgnoreCase) ||
             application.LearnerFullName.Contains(
                 search, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (filtered.Count == 0)
        {
            ApplicationCards.Add(new Label
            {
                Text = L.T("No applications found."),
                Padding = 16
            });

            return;
        }

        foreach (var application in filtered)
        {
            var body = new VerticalStackLayout { Spacing = 8 };

            body.Add(new Label
            {
                Text = application.ApplicationNumber,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            });

            body.Add(new Label
            {
                Text = application.LearnerFullName
            });

            body.Add(new Label
            {
                Text = application.GradeApplyingFor
            });

            body.Add(new Label
            {
                Text = L.T(application.Status),
                FontAttributes = FontAttributes.Bold,
                TextColor = string.Equals(
                    application.Status, "Approved",
                    StringComparison.OrdinalIgnoreCase)
                        ? Color.FromArgb("#187344")
                        : string.Equals(
                            application.Status, "Rejected",
                            StringComparison.OrdinalIgnoreCase)
                                ? Color.FromArgb("#B42318")
                                : Color.FromArgb("#9A6700")
            });

            var buttons = new HorizontalStackLayout { Spacing = 8 };

            AddAction(
                buttons,
                "View",
                () => Shell.Current.GoToAsync(
                    $"applicationdetail?id={application.Id}"));

            if (CanDecide(application))
            {
                AddAction(
                    buttons,
                    "Approve",
                    () => DecideAsync(application, "Approved"),
                    Color.FromArgb("#187344"));

                AddAction(
                    buttons,
                    "Reject",
                    () => DecideAsync(application, "Rejected"),
                    Color.FromArgb("#B42318"));
            }

            body.Add(buttons);

            ApplicationCards.Add(new Border
            {
                Padding = 16,
                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                Content = body
            });
        }
    }

    private void AddAction(
        HorizontalStackLayout row,
        string title,
        Func<Task> action,
        Color? color = null)
    {
        var button = new Button
        {
            Text = L.T(title),
            IsEnabled = !_busy
        };

        if (color is not null)
        {
            button.BackgroundColor = color;
            button.TextColor = Colors.White;
        }

        button.Clicked += async (_, _) => await RunAsync(action);

        _actions.Add(button);
        row.Add(button);
    }

    private static bool CanDecide(AdmissionApplication application) =>
        string.Equals(
            application.Status, "New",
            StringComparison.OrdinalIgnoreCase) ||
        string.Equals(
            application.Status, "Pending",
            StringComparison.OrdinalIgnoreCase);

    private async Task DecideAsync(
        AdmissionApplication application, string status)
    {
        if (!CanDecide(application))
            return;

        bool confirmed = await DisplayAlertAsync(
            L.T("Confirm"),
            L.T("Change this application status?"),
            L.T("Continue"),
            L.T("Cancel"));

        if (!confirmed)
            return;

        await _api.UpdateApplicationStatusAsync(application.Id, status);

        application.Status = status;
        Render();
    }
}