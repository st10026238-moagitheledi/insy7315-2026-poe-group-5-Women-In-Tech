namespace KoosMatliApp.Views.Admin;

public partial class Schooldetailspage : ContentPage
{
	public Schooldetailspage()
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