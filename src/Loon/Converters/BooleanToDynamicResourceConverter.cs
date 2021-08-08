using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Loon.Converters
{
    public class BooleanToDynamicResourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true &&
                parameter is DynamicResourceExtension { ResourceKey: { } } resource)
            {
                return Application.Current.TryFindResource(resource.ResourceKey, out var resourceValue)
                    ? resourceValue
                    : AvaloniaProperty.UnsetValue;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}