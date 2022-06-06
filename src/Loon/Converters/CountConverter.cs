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
            const double kb    = 1000;
            const double mb    = kb * kb;

            return count switch
            {
                0        => space,
                < 999    => count.ToString(CultureInfo.InvariantCulture),
                < 999999 => (count / kb).ToString("#.#K", CultureInfo.InvariantCulture),
                _        => (count / mb).ToString("#.#M", CultureInfo.InvariantCulture)
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}