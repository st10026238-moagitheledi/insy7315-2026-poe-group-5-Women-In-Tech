namespace KoosMatliApp.Views.Authentication;

public partial class SplashPage : ContentPage
{
    private bool _hasNavigated;

    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_hasNavigated)
            return;

        _hasNavigated = true;

        await Task.Delay(2000);

        if (Navigation.NavigationStack.Count > 0)
        {
            await Navigation.PushAsync(new LanguagePage());
        }
    }
}
