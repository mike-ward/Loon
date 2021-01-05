using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Loon.Models
{
    // This is neither performant or optimal. It is however convenient to use and for 99% of (my)
    // use cases, more than good enough.

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private static int nextId;
        private static readonly ConcurrentDictionary<string, object?> properties = new(StringComparer.Ordinal);

        private readonly string _id = Interlocked.Increment(ref nextId) + ".";

        protected T Getter<T>(T initialValue, [CallerMemberName] string? propertyName = null)
        {
            return (T)properties.GetOrAdd(_id + propertyName, initialValue)!;
        }

        protected void Setter<T>(T newValue, [CallerMemberName] string? propertyName = null)
        {
            var property = _id + propertyName;

            if (!properties.TryGetValue(property, out var oldValue) ||
                !EqualityComparer<T>.Default.Equals((T)oldValue, newValue))
            {
                properties[property] = newValue;
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