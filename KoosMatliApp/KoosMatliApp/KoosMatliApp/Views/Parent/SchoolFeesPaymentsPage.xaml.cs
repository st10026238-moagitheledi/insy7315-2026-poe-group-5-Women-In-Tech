using KoosMatliApp.Views.Parent;
using Microsoft.Maui.Controls.Shapes;
using System.Globalization;

namespace KoosMatliApp.Views.Parent;

public partial class SchoolFeesPaymentsPage : ContentPage
{
    private readonly List<LearnerFeeAccount> _accounts = new();

    public SchoolFeesPaymentsPage()
    {
        InitializeComponent();

        LoadSampleAccounts();
        LearnerPicker.ItemsSource = _accounts
            .Select(account => account.LearnerName)
            .ToList();

        if (LearnerPicker.Items.Count > 0)
            LearnerPicker.SelectedIndex = 0;
    }

    private void LoadSampleAccounts()
    {
        // Sample UI data only. Replace with the school finance API.
        _accounts.Clear();

        _accounts.Add(new LearnerFeeAccount
        {
            LearnerName = "Lerato Mokoena",
            TotalFees = 18000m,
            PaidToDate = 13500m,
            NextDueDate = new DateTime(2026, 10, 30),
            Payments =
            {
                new FeeStatementItem
                {
                    Title = "School fees - October",
                    Description = "Monthly school fee instalment",
                    Amount = 2500m,
                    Date = new DateTime(2026, 10, 30),
                    Status = "Outstanding"
                },
                new FeeStatementItem
                {
                    Title = "School fees - September",
                    Description = "Monthly school fee instalment",
                    Amount = 2000m,
                    Date = new DateTime(2026, 9, 30),
                    Status = "Paid"
                },
                new FeeStatementItem
                {
                    Title = "Registration fee",
                    Description = "Annual registration",
                    Amount = 500m,
                    Date = new DateTime(2026, 1, 15),
                    Status = "Paid"
                }
            }
        });

        _accounts.Add(new LearnerFeeAccount
        {
            LearnerName = "Thabo Mokoena",
            TotalFees = 16000m,
            PaidToDate = 12000m,
            NextDueDate = new DateTime(2026, 10, 30),
            Payments =
            {
                new FeeStatementItem
                {
                    Title = "School fees - October",
                    Description = "Monthly school fee instalment",
                    Amount = 2000m,
                    Date = new DateTime(2026, 10, 30),
                    Status = "Outstanding"
                },
                new FeeStatementItem
                {
                    Title = "School fees - September",
                    Description = "Monthly school fee instalment",
                    Amount = 2000m,
                    Date = new DateTime(2026, 9, 30),
                    Status = "Paid"
                }
            }
        });
    }

    private void OnLearnerChanged(object? sender, EventArgs e)
    {
        if (LearnerPicker.SelectedIndex < 0 ||
            LearnerPicker.SelectedIndex >= _accounts.Count)
            return;

        var account = _accounts[LearnerPicker.SelectedIndex];

        decimal outstanding = Math.Max(
            0m,
            account.TotalFees - account.PaidToDate);

        BalanceLabel.Text = FormatCurrency(outstanding);
        TotalFeesLabel.Text = FormatCurrency(account.TotalFees);
        PaidFeesLabel.Text = FormatCurrency(account.PaidToDate);
        DueDateLabel.Text = account.NextDueDate.ToString(
            "dd MMMM yyyy",
            CultureInfo.InvariantCulture);

        PaymentStatusLabel.Text = outstanding <= 0
            ? "Up to date"
            : "Payment due";

        PaymentStatusLabel.TextColor = outstanding <= 0
            ? Color.FromArgb("#86EFAC")
            : Color.FromArgb("#FDE68A");

        BalanceDescription.Text = "Sample data · Confirm with the school";

        RenderFeeItems(account);
    }

    private void RenderFeeItems(LearnerFeeAccount account)
    {
        FeeItemsContainer.Children.Clear();

        if (account.Payments.Count == 0)
        {
            FeeItemsContainer.Children.Add(new Label
            {
                Text = "No fee statements are available.",
                FontSize = 13,
                TextColor = Color.FromArgb("#64748B")
            });

            return;
        }

        foreach (var item in account.Payments.OrderByDescending(p => p.Date))
        {
            var isPaid = item.Status.Equals(
                "Paid",
                StringComparison.OrdinalIgnoreCase);

            var statusColor = isPaid
                ? Color.FromArgb("#15803D")
                : Color.FromArgb("#B45309");

            var statusBackground = isPaid
                ? Color.FromArgb("#DCFCE7")
                : Color.FromArgb("#FEF3C7");

            var title = new Label
            {
                Text = item.Title,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#172554")
            };

            var description = new Label
            {
                Text = item.Description,
                FontSize = 12,
                TextColor = Color.FromArgb("#64748B")
            };

            var amount = new Label
            {
                Text = FormatCurrency(item.Amount),
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#172554"),
                HorizontalTextAlignment = TextAlignment.End
            };

            var date = new Label
            {
                Text = item.Date.ToString(
                    "dd MMM yyyy",
                    CultureInfo.InvariantCulture),
                FontSize = 11,
                TextColor = Color.FromArgb("#64748B")
            };

            var status = new Label
            {
                Text = item.Status,
                FontSize = 10,
                FontAttributes = FontAttributes.Bold,
                TextColor = statusColor,
                BackgroundColor = statusBackground,
                Padding = new Thickness(9, 5),
                HorizontalOptions = LayoutOptions.End
            };

            var detailsButton = new Button
            {
                Text = "View details",
                FontSize = 12,
                TextColor = Color.FromArgb("#1D4ED8"),
                BackgroundColor = Colors.Transparent,
                Padding = new Thickness(0, 4),
                HorizontalOptions = LayoutOptions.Start
            };

            detailsButton.Clicked += async (_, _) =>
            {
                await DisplayAlertAsync(
                    item.Title,
                    $"Learner: {account.LearnerName}\n" +
                    $"Description: {item.Description}\n" +
                    $"Amount: {FormatCurrency(item.Amount)}\n" +
                    $"Date: {item.Date:dd MMMM yyyy}\n" +
                    $"Status: {item.Status}\n\n" +
                    "This is sample information. Please confirm " +
                    "your official statement with the school.",
                    "Close");
            };

            var leftColumn = new VerticalStackLayout
            {
                Spacing = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { title, description, date, detailsButton }
            };

            var rightColumn = new VerticalStackLayout
            {
                Spacing = 9,
                HorizontalOptions = LayoutOptions.End,
                Children = { amount, status }
            };

            var content = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                ColumnSpacing = 12
            };

            content.Add(leftColumn, 0, 0);
            content.Add(rightColumn, 1, 0);

            var card = new Border
            {
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E2E8F0"),
                StrokeThickness = 1,
                Padding = new Thickness(15),
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                Content = content
            };

            FeeItemsContainer.Children.Add(card);
        }
    }

    private static string FormatCurrency(decimal amount)
    {
        return amount.ToString(
            "C2",
            CultureInfo.GetCultureInfo("en-ZA"));
    }

    private async void OnHomeTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentDashboardPage());
    }

    private async void OnTimetableTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new TimetablePage());
    }

    private async void OnUpdatesTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AnnouncementsPage());
    }

    private async void OnChildrenTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MyChildrenPage());
    }

    private async void OnProfileTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ParentProfilePage());
    }
}

public class LearnerFeeAccount
{
    public string LearnerName { get; set; } = string.Empty;

    public decimal TotalFees { get; set; }

    public decimal PaidToDate { get; set; }

    public DateTime NextDueDate { get; set; }

    public List<FeeStatementItem> Payments { get; set; } = new();
}

public class FeeStatementItem
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = string.Empty;
}
