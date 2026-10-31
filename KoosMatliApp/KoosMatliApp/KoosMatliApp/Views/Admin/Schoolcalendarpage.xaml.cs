using KoosMatliApp.ViewModels;
namespace KoosMatliApp.Views.Admin;
public partial class Schoolcalendarpage : ContentPage
{
    public Schoolcalendarpage() { InitializeComponent(); BindingContext = App.Services!.GetRequiredService<SchoolDetailsViewModel>(); }
    protected override async void OnAppearing() { base.OnAppearing(); await ((SchoolDetailsViewModel)BindingContext).LoadCalendarEventsAsync(); }
}
