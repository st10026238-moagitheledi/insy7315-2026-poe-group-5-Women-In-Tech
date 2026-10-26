namespace KoosMatliApp.Views.Admin;

public partial class SchoolDatesPage : ContentPage
{
	public SchoolDatesPage()
	{
		InitializeComponent();
	}

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//admin/dashboard");
    }
}