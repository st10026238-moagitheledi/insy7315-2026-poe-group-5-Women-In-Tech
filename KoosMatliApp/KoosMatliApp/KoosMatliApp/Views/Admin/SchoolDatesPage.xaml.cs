using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class SchoolDatesPage : ContentPage
{
    public SchoolDatesPage() { 
        InitializeComponent(); 
        BindingContext = App.Services!.GetRequiredService<SchoolDatesViewModel>();
    } //(Microsoft Learn, 2026b).
    protected override async void OnAppearing() 
    { base.OnAppearing();
        await ((SchoolDatesViewModel)BindingContext).InitializeAsync();
    }//(Microsoft Learn, 2026a).
}

/*Reference List
 
 Microsoft Corporation, "Page Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle

 Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

 Microsoft Corporation, "Asynchronous Programming Patterns in .NET MAUI Apps," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/async
 */