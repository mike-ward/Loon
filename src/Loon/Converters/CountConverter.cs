using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class CountConverter : IValueConverter
    {
        [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int count)
            {
                return value;
            }

            const double k = 1000;

            return count switch
            {
                0        => " ",
                < 999    => count.ToString(CultureInfo.InvariantCulture),
                < 999999 => string.Format(CultureInfo.InvariantCulture, "{0:N1}K", count / k),
                _        => string.Format(CultureInfo.InvariantCulture, "{0:N1}M", count / (k * k))
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}