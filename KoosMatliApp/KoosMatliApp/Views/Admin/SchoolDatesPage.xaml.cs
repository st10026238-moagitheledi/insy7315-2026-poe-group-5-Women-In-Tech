namespace KoosMatliApp.Views.Admin;

public partial class SchoolDatesPage : ContentPage
{
	public SchoolDatesPage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.SchoolDetailsViewModel)) as KoosMatliApp.ViewModels.SchoolDetailsViewModel;
		}
		catch { }
	}
}