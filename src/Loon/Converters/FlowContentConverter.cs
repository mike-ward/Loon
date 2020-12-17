using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Services;
using Twitter.Models;

namespace Loon.Converters
{
    public class FlowContentConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TwitterStatus ts
                ? FlowContentService.FlowContentInlines(ts)
                : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}