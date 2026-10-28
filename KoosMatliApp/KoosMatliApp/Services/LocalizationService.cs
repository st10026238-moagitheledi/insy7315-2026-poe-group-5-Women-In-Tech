using KoosMatliApp.Models;
using System.ComponentModel;
using System.Globalization;

namespace KoosMatliApp.Services;

public sealed class LocalizationService : INotifyPropertyChanged
{
    public static LocalizationService Instance { get; } = new();

    private AppLanguage _language;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? LanguageChanged;

    private readonly Dictionary<string, string[]> _translations = new()
    {
        ["app_name"] = new[]
        {
            "Koos Matli Primary School",
            "Sekolo sa Phare sa Koos Matli",
            "Koos Matli Primary School"
        },
        ["motto"] = new[]
        {
            "Education is Light",
            "Thuto ke Lesedi",
            "Thuto ke Lesedi"
        },
        ["choose_language"] = new[]
        {
            "Choose your language",
            "Kgetha polelo ya gago",
            "Tlhopha puo ya gago"
        },
        ["language_description"] = new[]
        {
            "Select the language you would like to use.",
            "Kgetha polelo ye o ratago go e šomiša.",
            "Tlhopha puo e o ratang go e dirisa."
        },
        ["english"] = new[] { "English", "Seisimane", "Seesimane" },
        ["sepedi"] = new[] { "Sepedi", "Sepedi", "Sepedi" },
        ["setswana"] = new[] { "Setswana", "Setswana", "Setswana" },
        ["continue"] = new[] { "Continue", "Tšwela pele", "Tswelela" },
        ["next"] = new[] { "Next", "Latelago", "Latelang" },
        ["skip"] = new[] { "Skip", "Tshela", "Tlola" },
        ["get_started"] = new[] { "Get Started", "Thoma", "Simolola" },
        ["stay_connected"] = new[]
        {
            "Stay Connected",
            "Dula o kgokagane",
            "Nna o golagane"
        },
        ["stay_connected_desc"] = new[]
        {
            "Receive important school announcements and messages.",
            "Amogela ditsebišo le melaetša ya bohlokwa go tšwa sekolong.",
            "Amogela dikitsiso le melaetsa ya botlhokwa go tswa sekolong."
        },
        ["timetables"] = new[]
        {
            "Timetables & School Events",
            "Mananeo a thuto le ditiragalo tša sekolo",
            "Mananeo a thuto le ditiragalo tsa sekolo"
        },
        ["timetables_desc"] = new[]
        {
            "View timetables, upcoming events and important dates.",
            "Lebelela mananeo a thuto, ditiragalo le matšatši a bohlokwa.",
            "Bona mananeo a thuto, ditiragalo le matlha a botlhokwa."
        },
        ["offline"] = new[]
        {
            "Access Information Offline",
            "Hwetša tshedimošo le ge o se na inthanete",
            "Fitlhelela tshedimosetso le fa o se mo inthaneteng"
        },
        ["offline_desc"] = new[]
        {
            "Access key school information even when offline.",
            "Hwetša tshedimošo ya sekolo le ge o se na inthanete.",
            "Fitlhelela tshedimosetso ya sekolo le fa o se mo inthaneteng."
        },
        ["data_saver"] = new[]
        {
            "Data Saver Mode",
            "Mokgwa wa go boloka data",
            "Mokgwa wa go boloka data"
        },
        ["data_saver_desc"] = new[]
        {
            "Reduce data usage while staying connected.",
            "Fokotša tšhomišo ya data o dutše o kgokagane.",
            "Fokotsa tiriso ya data o ntse o golagane."
        },
        ["welcome_back"] = new[]
        {
            "Welcome Back!",
            "Re a go amogela gape!",
            "Re go amogela gape!"
        },
        ["login"] = new[] { "Log In", "Tsena", "Tsena" },
        ["register"] = new[]
        {
            "Create Parent Account",
            "Hlama akhaonto ya motswadi",
            "Tlhama akhaonto ya motsadi"
        },
        ["full_name"] = new[] { "Full name", "Leina ka botlalo", "Maina ka botlalo" },
        ["email"] = new[] { "Email address", "Aterese ya imeile", "Aterese ya imeile" },
        ["password"] = new[] { "Password", "Phasewete", "Lefoko-kunutu" },
        ["confirm_password"] = new[]
        {
            "Confirm password",
            "Netefatša phasewete",
            "Netefatsa lefoko-kunutu"
        },
        ["forgot_password"] = new[]
        {
            "Forgot Password?",
            "O lebetše phasewete?",
            "O lebetse lefoko-kunutu?"
        },
        ["send_reset"] = new[]
        {
            "Send Reset Link",
            "Romela kgokagano ya go mpshafatša",
            "Romela kgokagano ya go seta sesha"
        },
        ["back_login"] = new[]
        {
            "Back to Login",
            "Boela go tsena",
            "Boela kwa go tsena"
        },
        ["parent_only"] = new[]
        {
            "Parents can register here. Teachers and admins receive accounts from the school.",
            "Batswadi ba ka ingwadiša mo. Barutiši le balaodi ba hwetša diakhaonto go tšwa sekolong.",
            "Batsadi ba ka ikwadisa fano. Barutabana le batsamaisi ba newa diakhaonto ke sekolo."
        },
        ["required_fields"] = new[]
        {
            "Please complete all required fields.",
            "Tlatša mafelo ka moka a a nyakegago.",
            "Tsweetswee tlatsa mafelo otlhe a a tlhokegang."
        },
        ["password_mismatch"] = new[]
        {
            "Your passwords do not match.",
            "Diphasewete ga di swane.",
            "Mafoko-kunutu ga a tshwane."
        },
        ["password_length"] = new[]
        {
            "Password must contain at least 8 characters.",
            "Phasewete e swanetše go ba le ditlhaka tše 8 goba go feta.",
            "Lefoko-kunutu le tshwanetse go nna le ditlhaka di le 8 kgotsa go feta."
        },
        ["registration_success"] = new[]
        {
            "Account created successfully. You can now sign in.",
            "Akhaonto e hlamilwe. Bjale o ka tsena.",
            "Akhaonto e tlhamilwe. Jaanong o ka tsena."
        },
        ["connection_error"] = new[]
        {
            "Unable to connect to the server. Please try again.",
            "Ga go kgonege go kgokagana le seva. Leka gape.",
            "Ga go kgonege go golagana le seva. Leka gape."
        },
        ["ok"] = new[] { "OK", "Go lokile", "Go siame" }
    };

    private LocalizationService()
    {
        string savedLanguage = Preferences.Get(
            "app_language",
            nameof(AppLanguage.English));

        _language = Enum.TryParse(
            savedLanguage, out AppLanguage saved)
            ? saved
            : AppLanguage.English;

        ApplyCulture();
    }

    public AppLanguage Language => _language;

    public string this[string key]
    {
        get
        {
            if (_translations.TryGetValue(key, out var values))
                return values[(int)_language];

            return key;
        }
    }

    public void SetLanguage(AppLanguage language)
    {
        if (_language == language)
            return;

        _language = language;

        Preferences.Set("app_language", language.ToString());

        ApplyCulture();

        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nameof(Language)));

        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyCulture()
    {
        string cultureName = _language switch
        {
            AppLanguage.Sepedi => "nso-ZA",
            AppLanguage.Setswana => "tn-ZA",
            _ => "en-ZA"
        };

        var culture = CultureInfo.GetCultureInfo(cultureName);

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}
