using Microsoft.Maui.Controls.Xaml;

namespace KoosMatliApp.Resources.Localization;

[ContentProperty(nameof(Key))]
public class TranslateExtension :
    IMarkupExtension<BindingBase>
{
    public string Key { get; set; } =
        string.Empty; // (Microsoft, 2026)


    public BindingBase ProvideValue(
        IServiceProvider serviceProvider)
    {
        return new Binding(
            $"[{Key}]",
            source:
                LocalizationManager.Instance);
    }


    object IMarkupExtension.ProvideValue(
        IServiceProvider serviceProvider)
    {
        return ProvideValue(
            serviceProvider); // (Microsoft, 2026)
    }
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

