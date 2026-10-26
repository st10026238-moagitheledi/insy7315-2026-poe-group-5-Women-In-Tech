namespace KoosMatliApp.Views.Admin;

public partial class Schoolcalendarpage : ContentPage
{
	public Schoolcalendarpage()
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