using Microsoft.Maui.Controls.Xaml;

namespace KoosMatliApp.Resources.Localization;

[ContentProperty(nameof(Key))]
public class TranslateExtension :
    IMarkupExtension<BindingBase>
{
    public string Key { get; set; } =
        string.Empty;


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
            serviceProvider);
    }
}
