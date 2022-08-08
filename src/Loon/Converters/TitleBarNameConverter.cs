using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Loon.Interfaces;
using Loon.Models;

namespace Loon.Converters;

internal sealed class TitleBarNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var title = App.GetString("title");
        if (value is string screenName 
            && string.IsNullOrWhiteSpace(screenName) is false 
            && App.Settings.HideNameInTitleBar is false)
        {
            title = $"{title} - {screenName}";
        }
        return title;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}