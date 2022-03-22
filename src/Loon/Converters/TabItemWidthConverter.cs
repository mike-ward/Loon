using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal sealed class TabItemWidthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is double val
                ? val / 5.25 // yep, magic number. Kept fiddling till I got something that looked good.
                : value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}