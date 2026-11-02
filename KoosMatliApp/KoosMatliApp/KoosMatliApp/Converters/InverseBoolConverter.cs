using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace KoosMatliApp.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) //(Microsoft Learn, 2026a).
    {
        return value is bool b && !b; //(Microsoft Learn, 2026b).
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) //(Microsoft Learn, 2026a).
    {
        return value is bool b && !b;
    }
}

/*
 Reference List
Microsoft Corporation, "Data Binding Converters in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/xaml/fundamentals/data-binding-converters

Microsoft Corporation, "Pattern Matching (C# Programming Guide)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
 */
