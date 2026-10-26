namespace KoosMatliApp.Views.Admin;

public partial class Documentspage : ContentPage
{
	public Documentspage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.DocumentsViewModel)) as KoosMatliApp.ViewModels.DocumentsViewModel;
		}
		catch { }
	}
}