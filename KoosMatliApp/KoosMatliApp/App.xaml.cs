using KoosMatliApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KoosMatliApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Initialize local DB in background
            // Source: Initialize DB in background to avoid blocking UI thread (async pattern) [2] (2026)
            _ = Task.Run(async () => await UserRepository.InitializeAsync());

            // Start with the loading page which will transition to onboarding
            MainPage = new KoosMatliApp.MainPage();
        }
    }
}