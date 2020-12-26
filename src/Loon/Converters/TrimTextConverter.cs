using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal class TrimTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string ?? string.Empty;
            const int maxLength = 300;

            return text.Length > maxLength
                ? text.Substring(0, maxLength) + "…"
                : text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}