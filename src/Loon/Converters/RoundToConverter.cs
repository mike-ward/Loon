using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    public class RoundToConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var digits = parameter is int p
                ? p
                : 1;

            return value is double d
                // ReSharper disable once HeapView.BoxingAllocation
                ? Math.Round(d, digits)
                : value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}