namespace KoosMatliApp.Views.Admin;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
		try
		{
			var services = App.Services;
			BindingContext = services?.GetService(typeof(KoosMatliApp.ViewModels.UsersViewModel)) as KoosMatliApp.ViewModels.UsersViewModel;
		}
		catch { }
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			if (BindingContext is KoosMatliApp.ViewModels.UsersViewModel vm)
			{
				await vm.InitializeAsync();
			}
		}
		catch { }
	}
}