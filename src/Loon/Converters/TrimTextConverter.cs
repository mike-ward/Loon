using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Extensions;

namespace Loon.Converters
{
    internal sealed class TrimTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var       text      = value as string ?? string.Empty;
            const int maxLength = 300;
            return text.TruncateWithEllipsis(maxLength);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}