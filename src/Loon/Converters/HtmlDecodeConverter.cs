using System;
using System.Globalization;
using System.Net;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    public class HtmlDecodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return value is string text
                ? WebUtility.HtmlDecode(WebUtility.HtmlDecode(text)) ?? string.Empty
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}