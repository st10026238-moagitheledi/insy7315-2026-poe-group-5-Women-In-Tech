using KoosMatliApp.Pages;
using KoosMatliApp.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using System;

namespace KoosMatliApp
{
    public partial class MainPage : ContentPage
    {
        private bool _isLoading;
        private bool _hasStarted;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Prevent the loading sequence from running twice.
            if (_hasStarted)
                return;

            _hasStarted = true;
            _isLoading = true;

            // Start the logo pulse animation.
            _ = PulseLogoAsync();

            // Keep the loading screen visible for four seconds.
            await Task.Delay(4000);

            // Stop the animation before navigating.
            _isLoading = false;
            LogoFrame.CancelAnimations();
            await LogoFrame.ScaleTo(1.0, 150, Easing.SinOut);

            // Continue to language selection.
            Application.Current!.MainPage = new NavigationPage(new LanguagePage());
        }

        private async Task PulseLogoAsync()
        {
            while (_isLoading)
            {
                await LogoFrame.ScaleTo(1.06, 500, Easing.SinInOut);

                if (!_isLoading)
                    break;

                await LogoFrame.ScaleTo(0.98, 500, Easing.SinInOut);
            }
        }
    }
}

// References:
// Microsoft Learn - .NET MAUI Preferences - https://learn.microsoft.com/dotnet/maui/platform-integration/storage/preferences [Accessed 2026]
// sqlite-net-pcl (NuGet) - package and usage: https://www.nuget.org/packages/sqlite-net-pcl [Accessed 2026]
// Microsoft Learn - Local databases in .NET MAUI - https://learn.microsoft.com/dotnet/maui/data-cloud/local-databases [Accessed 2026]
//Microsoft Learn, 2026. Custom animation triggers inside .NET MAUI views. [online] Available at: <https://learn.microsoft.com/en-us/dotnet/maui/user-interface/animation/custom?view=net-maui-10.0> [Accessed 27 October 2026].





