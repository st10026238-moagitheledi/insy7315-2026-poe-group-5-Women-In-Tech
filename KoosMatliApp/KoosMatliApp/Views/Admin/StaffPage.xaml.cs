namespace KoosMatliApp.Views.Admin;

public partial class StaffPage : ContentPage
{
	public StaffPage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.StaffViewModel)) as KoosMatliApp.ViewModels.StaffViewModel;
		}
		catch { }
	}
}