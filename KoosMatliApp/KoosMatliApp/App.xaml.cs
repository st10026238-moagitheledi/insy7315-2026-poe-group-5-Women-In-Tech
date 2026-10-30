namespace KoosMatliApp;
using KoosMatliApp.Resources.Localization;

public partial class App : Application
{
    public App()
    {
        LocalizationManager.Instance.Initialize();

        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}