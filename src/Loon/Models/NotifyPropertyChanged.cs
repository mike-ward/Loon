using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Loon.Models
{
    // This is neither performant or optimal. It is however convenient to use and for 99% of (my)
    // use cases, more than good enough.

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private readonly string _id = Path.GetRandomFileName() + ".";
        private static readonly ConcurrentDictionary<string, object?> _properties = new(StringComparer.Ordinal);

        protected T Getter<T>(T initialValue, [CallerMemberName] string? propertyName = null)
        {
            return (T)_properties.GetOrAdd(_id + propertyName, initialValue) ?? initialValue;
        }

        protected void Setter<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            var key = _id + propertyName;
            if (!_properties.TryGetValue(key, out var val) ||
                !EqualityComparer<T>.Default.Equals((T)val, value))
            {
                _properties[key] = value;
                OnPropertyChanged(propertyName);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}