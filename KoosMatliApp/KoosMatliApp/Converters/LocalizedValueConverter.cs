using System.Globalization;
namespace KoosMatliApp.Converters;
public sealed class LocalizedValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => L.T(value?.ToString() ?? "");
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
public sealed class LocalTimeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is DateTime time ? time.ToLocalTime().ToString("g", L.Culture) : "";
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
