using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Loon.Converters
{
    internal sealed class TimeAgoConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var time = value is DateTime val
                ? val
                : default;

            var timespan = DateTime.UtcNow - time;

            return timespan switch
            {
                { TotalSeconds: < 60 } => Format("0s", timespan.TotalSeconds),
                { TotalMinutes: < 60 } => Format("0m", timespan.TotalMinutes),
                { TotalHours  : < 24 } => Format("0h", timespan.TotalHours),
                { TotalDays   : < 03 } => Format("0d", timespan.TotalDays),
                _                      => time.ToString("MMM d", CultureInfo.CurrentUICulture)
            };
        }

        private static string Format(string format, double total)
        {
            return ((int)total).ToString(format, CultureInfo.InvariantCulture);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}