using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class Schooldetailspage : ContentPage
{
    public Schooldetailspage() { InitializeComponent(); BindingContext = App.Services!.GetRequiredService<SchoolDetailsViewModel>(); }
    protected override async void OnAppearing() { base.OnAppearing(); await ((SchoolDetailsViewModel)BindingContext).LoadAsync(); }
}
