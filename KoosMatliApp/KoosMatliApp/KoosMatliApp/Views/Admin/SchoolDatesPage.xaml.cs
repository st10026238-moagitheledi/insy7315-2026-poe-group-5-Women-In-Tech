using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class SchoolDatesPage : ContentPage
{
    public SchoolDatesPage() { InitializeComponent(); BindingContext = App.Services!.GetRequiredService<SchoolDatesViewModel>(); }
    protected override async void OnAppearing() { base.OnAppearing(); await ((SchoolDatesViewModel)BindingContext).InitializeAsync(); }
}
