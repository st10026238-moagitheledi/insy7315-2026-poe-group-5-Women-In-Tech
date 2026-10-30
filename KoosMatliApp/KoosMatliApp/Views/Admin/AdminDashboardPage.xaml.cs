using KoosMatliApp.ViewModels;

namespace KoosMatliApp.Views.Admin;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();

        BindingContext = App.Services?
          .GetService(typeof(AdminDashboardViewModel))
          as AdminDashboardViewModel;

        var btnDetails = FindByName("btnSchoolDetails") as Button;
        var btnDates = FindByName("btnSchoolDates") as Button;
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
              await Shell.Current.GoToAsync("schooldates");
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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AdminDashboardViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}