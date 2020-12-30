using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Loon.Models
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        // Yeah, there's boxing going on here.

        private readonly string _id = Path.GetRandomFileName(); // easy way to get a unique string
        private static readonly ConcurrentDictionary<string, object?> _properties = new();

        protected T? GetProp<T>([CallerMemberName] string? propertyName = null)
        {
            return _properties.TryGetValue(_id + propertyName, out var property)
                ? (T)property
                : default;
        }

        protected void SetProp<T>(T? value, [CallerMemberName] string? propertyName = null)
        {
            var key = _id + propertyName;

            if (!_properties.TryGetValue(key, out var property) ||
                !EqualityComparer<T>.Default.Equals((T)property, value))
            {
                _properties[key] = value;
                OnPropertyChanged(propertyName!);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}