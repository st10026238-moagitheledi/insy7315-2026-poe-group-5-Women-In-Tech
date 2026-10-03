using KoosMatliApp.Features.Teacher.Views;

namespace KoosMatliApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(
            nameof(ClassDetailsPage),
            typeof(ClassDetailsPage));
    }
}
