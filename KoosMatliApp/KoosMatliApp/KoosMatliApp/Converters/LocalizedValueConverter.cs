using System.Globalization;
namespace KoosMatliApp.Converters;
public sealed class LocalizedValueConverter : IValueConverter //(Microsoft Learn, 2026a).
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => L.T(value?.ToString() ?? "");
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
public sealed class LocalTimeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is DateTime time ? time.ToLocalTime().ToString("g", L.Culture) : ""; //(Microsoft Learn, 2026b; Microsoft Learn, 2026c).
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
/*Reference List
 
 Microsoft Corporation, "Data Binding Converters in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/data-binding-converters

 Microsoft Corporation, "Globalize and Localize .NET Applications," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/core/extensions/localization

 Microsoft Corporation, "Formatting Dates and Times with CultureInfo in .NET," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/standard/base-types/formatting-types
 */