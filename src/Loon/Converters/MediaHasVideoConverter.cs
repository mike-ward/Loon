using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Extensions;
using Twitter.Models;

namespace Loon.Converters
{
    internal class MediaHasVideoConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Services.ImageService.VideoUrl(value as Media).IsNotNullOrWhiteSpace();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}