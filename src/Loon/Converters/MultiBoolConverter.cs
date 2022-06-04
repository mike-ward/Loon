using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class MultiBoolConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            return values.All(value => value is bool and true);
        }
    }
}