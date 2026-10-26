using System;
using System.Collections.Generic;
using System.Text;

namespace KoosMatliApp.Views.Admin
{
    public partial class AdminDashboardPage: ContentPage
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
            try
            {
                var services = App.Services;
                BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.AdminDashboardViewModel)) as KoosMatliApp.ViewModels.AdminDashboardViewModel;
            }
            catch
            {
            }
            // Attach click handlers for quick action buttons
            try
            {
                var btnDetails = this.FindByName<Button>("btnSchoolDetails");
                var btnDates = this.FindByName<Button>("btnSchoolDates");
                var btnCalendar = this.FindByName<Button>("btnSchoolCalendar");
                var btnDocs = this.FindByName<Button>("btnDocuments");

                if (btnDetails != null) btnDetails.Clicked += async (s, e) => await Shell.Current.GoToAsync("admin/school/details");
                if (btnDates != null) btnDates.Clicked += async (s, e) => await Shell.Current.GoToAsync("admin/school/dates");
                if (btnCalendar != null) btnCalendar.Clicked += async (s, e) => await Shell.Current.GoToAsync("admin/school/calendar");
                if (btnDocs != null) btnDocs.Clicked += async (s, e) => await Shell.Current.GoToAsync("admin/documents");
            }
            catch { }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (BindingContext is KoosMatliApp.ViewModels.AdminDashboardViewModel vm)
                {
                    await vm.InitializeAsync();
                }
            }
            catch { }
        }
    }
}
