using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class Applicationsdetailspage : ContentPage, IQueryAttributable
{
    private int _id;
    public Applicationsdetailspage() { InitializeComponent(); BindingContext = App.Services!.GetRequiredService<ApplicationsDetailViewModel>(); } //(Microsoft Learn, 2026b).
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    { //(Microsoft Learn, 2026a).
        _id = query.TryGetValue("id", out var value) && int.TryParse(value?.ToString(), out var id) && id > 0 ? id : 0;
    }
    protected override async void OnAppearing() {
        base.OnAppearing();
        if (BindingContext is ApplicationsDetailViewModel vm) {
            if (_id > 0) await vm.LoadAsync(_id); //(Microsoft Learn, 2026c).
            else { vm.HasError = true; vm.ErrorMessage = L.T("Choose an application first."); }
        }
    }
}

/*Reference List
 
Microsoft Corporation, "Pass Data Using Query Parameters in .NET MAUI Shell," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation#pass-data

Microsoft Corporation, "Dependency Injection in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/dependency-injection

Microsoft Corporation, "Page Lifecycle in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle
 */
