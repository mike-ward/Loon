﻿using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.MarkupExtensions;

// ReSharper disable HeapView.BoxingAllocation

namespace Loon.Converters
{
    internal sealed class BooleanToDynamicResourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is true &&
                parameter is DynamicResourceExtension { ResourceKey: { } } resource)
            {
                return Application.Current is not null
                    && Application.Current.TryFindResource(resource.ResourceKey, out var resourceValue)
                    ? resourceValue
                    : AvaloniaProperty.UnsetValue;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}