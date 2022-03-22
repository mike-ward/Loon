using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal sealed class IsHiddenImageConverter : IMultiValueConverter
    {
        public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            return values.Count != 2
                || values[0] is not string url
                || values[1] is not IReadOnlySet<string> readOnlySet
                || !readOnlySet.Contains(url);
        }
    }
}