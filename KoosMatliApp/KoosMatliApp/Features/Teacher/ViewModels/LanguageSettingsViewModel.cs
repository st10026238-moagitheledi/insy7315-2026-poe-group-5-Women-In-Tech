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
        };


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
    }


    public void SaveLanguage()
    {
        TeacherSettingsService.UpdateLanguage(
            SelectedLanguage);


        LocalizationManager.Instance.SetLanguage(
            SelectedLanguage);
    }
}