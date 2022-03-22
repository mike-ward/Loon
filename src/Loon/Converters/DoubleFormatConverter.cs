using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal sealed class DoubleFormatConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var format = parameter as string ?? "N";
            return ((double)(value ?? double.MaxValue)).ToString(format, culture);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value, culture);
        }
    }
}