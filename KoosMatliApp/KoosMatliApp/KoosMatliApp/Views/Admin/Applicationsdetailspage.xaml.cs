using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class Applicationsdetailspage : ContentPage, IQueryAttributable
{
    private int _id;
    public Applicationsdetailspage() { InitializeComponent(); BindingContext = App.Services!.GetRequiredService<ApplicationsDetailViewModel>(); }
    public void ApplyQueryAttributes(IDictionary<string, object> query) {
        _id = query.TryGetValue("id", out var value) && int.TryParse(value?.ToString(), out var id) && id > 0 ? id : 0;
    }
    protected override async void OnAppearing() {
        base.OnAppearing();
        if (BindingContext is ApplicationsDetailViewModel vm) {
            if (_id > 0) await vm.LoadAsync(_id);
            else { vm.HasError = true; vm.ErrorMessage = L.T("Choose an application first."); }
        }
    }
}
