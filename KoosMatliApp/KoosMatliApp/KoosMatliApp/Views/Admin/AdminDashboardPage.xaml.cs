using KoosMatliApp.ViewModels;

namespace KoosMatliApp.Views.Admin;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();

        BindingContext = App.Services?
          .GetService(typeof(AdminDashboardViewModel)) //(Microsoft Learn, 2026a).
          as AdminDashboardViewModel;

        var btnDetails = FindByName("btnSchoolDetails") as Button;
        var btnDates = FindByName("btnSchoolDates") as Button; //(Microsoft Learn, 2026a).
        var btnCalendar = FindByName("btnSchoolCalendar") as Button;
        var btnDocs = FindByName("btnDocuments") as Button;

        if (btnDetails != null)
        {
            btnDetails.Clicked += async (s, e) =>
              await Shell.Current.GoToAsync("schooldetails");
        }

        if (btnDates != null)
        {
            btnDates.Clicked += async (s, e) =>
              await Shell.Current.GoToAsync("schooldates"); //(Microsoft Learn, 2026b).
        }

        if (btnCalendar != null)
        {
            btnCalendar.Clicked += async (s, e) =>
              await Shell.Current.GoToAsync("schoolcalendar");
        }

        if (btnDocs != null)
        {
            btnDocs.Clicked += async (s, e) =>
              await Shell.Current.GoToAsync("documents");
        }
    }

    protected override async void OnAppearing() //(Microsoft Learn, 2026c).
    {
        base.OnAppearing();

        if (BindingContext is AdminDashboardViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}

/*Reference List
 Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

 Microsoft Corporation, "Shell Navigation in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation

 Microsoft Corporation, "Page Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle
 */