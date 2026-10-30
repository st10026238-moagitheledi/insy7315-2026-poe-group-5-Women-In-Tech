using KoosMatliApp.Models;
using Microsoft.Maui.Controls.Shapes;

namespace KoosMatliApp.Views.Admin;

public sealed class UserDetailsPage : ContentPage
{
    private bool _closing;

    public UserDetailsPage(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        Title = Words(
            "User Details",
            "Dintlha tsa modirisi",
            "Dintlha tša modiriši");

        Shell.SetNavBarIsVisible(this, false);

        SafeAreaEdges = new Microsoft.Maui.SafeAreaEdges(
            Microsoft.Maui.SafeAreaRegions.Container);

        var back = new Button
        {
            Text = L.T("← Back"),
            HorizontalOptions = LayoutOptions.Start
        };

        back.Clicked += async (_, _) =>
        {
            if (_closing)
                return;

            _closing = true;

            try
            {
                await Navigation.PopModalAsync();
            }
            finally
            {
                _closing = false;
            }
        };

        var heading = new HorizontalStackLayout
        {
            Padding = new Thickness(16, 12),
            Spacing = 12,
            Children =
            {
                back,
                new Label
                {
                    Text = Title,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center
                }
            }
        };

        var details = new VerticalStackLayout { Spacing = 12 };

        AddField(
            details,
            Words("Full name", "Leina ka botlalo", "Leina ka botlalo"),
            user.FullName);

        AddField(details, L.T("Role"), L.T(user.Role));
        AddField(details, L.T("Email address"), user.Email);
        AddField(details, L.T("Phone number"), user.PhoneNumber);

        var content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 16,
            Children =
            {
                new Border
                {
                    Padding = 20,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 12
                    },
                    Content = details
                }
            }
        };

        var layout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            }
        };

        layout.Add(heading, 0, 0);
        layout.Add(new ScrollView { Content = content }, 0, 1);

        Content = layout;
    }

    private static void AddField(
        VerticalStackLayout details,
        string label,
        string? value)
    {
        details.Add(new Label
        {
            Text = label,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold
        });

        details.Add(new Label
        {
            Text = string.IsNullOrWhiteSpace(value) ? "—" : value.Trim(),
            FontSize = 16
        });
    }

    private static string Words(
        string english, string setswana, string sepedi) =>
        L.Culture.Name switch
        {
            "tn-ZA" => setswana,
            "nso-ZA" => sepedi,
            _ => english
        };
}