using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class CountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int count)
            {
                return value;
            }

            const string space = " ";
            const double k     = 1000;
            const double m     = k * k;

            return count switch
            {
                0        => space,
                < 999    => count.ToString(CultureInfo.InvariantCulture),
                < 999999 => (count / k).ToString("#.#K", CultureInfo.InvariantCulture),      
                _        => (count / m).ToString("#.#M", CultureInfo.InvariantCulture) 
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}