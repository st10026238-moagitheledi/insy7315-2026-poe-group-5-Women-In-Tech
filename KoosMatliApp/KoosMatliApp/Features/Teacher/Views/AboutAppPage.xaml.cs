namespace KoosMatliApp.Features.Teacher.Views;

public partial class AboutAppPage : ContentPage
{
    public AboutAppPage()
    {
        InitializeComponent();
    }


    private async void OnBackTapped(
        object sender,
        TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}