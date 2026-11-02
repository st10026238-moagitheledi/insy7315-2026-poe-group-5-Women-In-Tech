using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;

public partial class Schoolcalendarpage : ContentPage
{
    public Schoolcalendarpage()
    {
        InitializeComponent();
        BindingContext = App.Services!.GetRequiredService<SchoolDetailsViewModel>();
    } //(Microsoft Learn, 2026a).
    protected override async void OnAppearing()
    { //(Microsoft Learn, 2026a).
        base.OnAppearing(); await ((SchoolDetailsViewModel)BindingContext).LoadCalendarEventsAsync();
    } //(Microsoft Learn, 2026b).
}

/*Reference List
 
Microsoft Corporation, "Page Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle

Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async
 */