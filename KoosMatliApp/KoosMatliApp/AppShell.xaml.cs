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

        Routing.RegisterRoute(
            nameof(HomeworkPage),
            typeof(HomeworkPage));

        Routing.RegisterRoute(
            nameof(AddHomeworkPage),
            typeof(AddHomeworkPage));
    }
}
