namespace KoosMatliApp.Views.Admin;

public partial class applicationspage : ContentPage
{
	public applicationspage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.ApplicationsViewModel)) as KoosMatliApp.ViewModels.ApplicationsViewModel;
		}
		catch { }
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			if (BindingContext is KoosMatliApp.ViewModels.ApplicationsViewModel vm)
			{
				await vm.LoadAsync();
			}
		}
		catch { }
	}
}