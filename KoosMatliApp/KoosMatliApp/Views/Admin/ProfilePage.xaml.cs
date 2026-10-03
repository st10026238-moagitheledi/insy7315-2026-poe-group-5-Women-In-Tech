namespace KoosMatliApp.Views.Admin;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			// Profile currently can use AdminDashboardViewModel or its own in future
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.AdminDashboardViewModel)) as KoosMatliApp.ViewModels.AdminDashboardViewModel;
		}
		catch { }
	}
}