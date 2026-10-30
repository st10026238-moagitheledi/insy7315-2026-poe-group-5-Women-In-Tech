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
        }
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
    }


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
                "Item[]"));
    }
}