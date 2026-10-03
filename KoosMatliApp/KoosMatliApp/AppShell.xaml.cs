namespace KoosMatliApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Register routes for quick navigation targets (use admin/ prefix to avoid ambiguous relative routing)
            Routing.RegisterRoute("admin/school/details", typeof(Views.Admin.Schooldetailspage));
            Routing.RegisterRoute("admin/school/dates", typeof(Views.Admin.SchoolDatesPage));
            Routing.RegisterRoute("admin/school/calendar", typeof(Views.Admin.Schoolcalendarpage));
            Routing.RegisterRoute("admin/documents", typeof(Views.Admin.Documentspage));
        }
    }
}
