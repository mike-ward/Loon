using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    public class HeartFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val && parameter is string par)
            {
                var multiplier = System.Convert.ToDouble(par);
                return val * multiplier;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}