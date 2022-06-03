using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class DoubleFormatConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var format = parameter as string ?? "N";
            return (value is double val
                    ? val
                    : double.MaxValue)
               .ToString(format, culture);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            return System.Convert.ToDouble(value, culture);
        }
    }
}