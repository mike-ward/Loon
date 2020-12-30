using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Services;

namespace Loon.Converters
{
    internal sealed class ViewModelLocator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Type ?? throw new ArgumentException("value is not a type", nameof(value));
            return BootStrapper.GetService(type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // not used
        }
    }
}