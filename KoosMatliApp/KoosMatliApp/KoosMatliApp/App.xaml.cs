using KoosMatliApp.Resources.Localization;
using KoosMatliApp.Views.Authentication;

namespace KoosMatliApp;

public partial class App : Application
{
    public App()
    {
        LocalizationManager.Instance.Initialize();

        InitializeComponent();
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        return new Window(
            new NavigationPage(
                new SplashPage())
        );
    }
}