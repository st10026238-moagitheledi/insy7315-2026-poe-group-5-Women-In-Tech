using KoosMatliApp.Features.Teacher.Views;

namespace KoosMatliApp.Features.Teacher;

public partial class TeacherShell : Shell
{
    public TeacherShell()
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
            typeof(AboutAppPage));
    }
}