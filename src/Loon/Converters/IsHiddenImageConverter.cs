using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal class IsHiddenImageConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Count != 2 
                || values[0] is not string url 
                || values[1] is not IReadOnlySet<string> readOnlySet 
                || !readOnlySet.Contains(url);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Array.Empty<object>();
        }
    }
}