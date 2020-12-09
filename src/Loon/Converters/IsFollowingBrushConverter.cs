using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    public class IsFollowingBrushConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool val && val
                ? Avalonia.Application.Current.FindResource("TwitterBlueBrush")
                : Avalonia.Application.Current.FindResource("ThemeForegroundBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}