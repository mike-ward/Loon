using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Extensions;
using Loon.Services;
using Twitter.Models;

namespace Loon.Converters
{
    [SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
    internal sealed class MediaHasVideoConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ImageService.VideoUrl(value as Media).IsNotNullOrWhiteSpace();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}