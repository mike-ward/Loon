using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class ShortLinkConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool and true
                ? App.GetString("short-link-text")
                : parameter;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}