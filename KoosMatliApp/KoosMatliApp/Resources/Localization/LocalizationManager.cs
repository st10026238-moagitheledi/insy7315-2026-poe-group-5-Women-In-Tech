using System.ComponentModel;
using System.Globalization;

namespace KoosMatliApp.Resources.Localization;

public class LocalizationManager : INotifyPropertyChanged
{
    private static readonly LocalizationManager _instance =
        new();


    public static LocalizationManager Instance =>
        _instance;


    private CultureInfo _culture =
        new("en-ZA");


    public CultureInfo Culture =>
        _culture;


    public string this[string key]
    {
        get
        {
            return AppResources.ResourceManager.GetString(
                       key,
                       _culture)
                   ?? key;
        } // (Microsoft, 2026)
    }


    public event PropertyChangedEventHandler?
        PropertyChanged;


    public void Initialize()
    {
        var selectedLanguage =
            Features.Teacher.Services
                .TeacherSettingsService
                .GetSettings()
                .Language;


        SetLanguage(
            selectedLanguage);
    } // (Microsoft, 2026)


    public void SetLanguage(
        string language)
    {
        var cultureCode =
            language switch
            {
                "Setswana" =>
                    "tn-ZA",

                "Sepedi" =>
                    "nso-ZA",

                _ =>
                    "en-ZA"
            };


        SetCulture(
            cultureCode);
    }


    private void SetCulture(
        string cultureCode)
    {
        _culture =
            new CultureInfo(
                cultureCode);


        CultureInfo.CurrentCulture =
            _culture;

        CultureInfo.CurrentUICulture =
            _culture;

        CultureInfo.DefaultThreadCurrentCulture =
            _culture;

        CultureInfo.DefaultThreadCurrentUICulture =
            _culture;


        AppResources.Culture =
            _culture;


        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(
                null));


        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(
                "Item[]")); // (Microsoft, 2026)
    }
}

//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].
