using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KoosMatliApp.Features.Teacher.Services;
using KoosMatliApp.Resources.Localization;

namespace KoosMatliApp.Features.Teacher.ViewModels;

public class LanguageSettingsViewModel :
    ObservableObject
{
    public ObservableCollection<string>
        Languages
    { get; } =
        new()
        {
            "English",
            "Setswana",
            "Sepedi"
        }; // (Microsoft, 2026)


    private string _selectedLanguage =
        "English";

    public string SelectedLanguage
    {
        get => _selectedLanguage;

        set => SetProperty(
            ref _selectedLanguage,
            value);
    }


    public void LoadLanguage()
    {
        SelectedLanguage =
            TeacherSettingsService
                .GetSettings()
                .Language;
    } // (Microsoft, 2026)


    public void SaveLanguage()
    {
        TeacherSettingsService.UpdateLanguage(
            SelectedLanguage);


        LocalizationManager.Instance.SetLanguage(
            SelectedLanguage);
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

