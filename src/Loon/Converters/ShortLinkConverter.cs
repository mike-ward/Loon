using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal sealed class ShortLinkConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true
                ? App.GetString("short-link-text")
                : parameter;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}