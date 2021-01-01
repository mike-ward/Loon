using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Loon.Models
{
    // Yeah, there's boxing going on here.

    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private readonly string _id = Path.GetRandomFileName() + ".";
        private static readonly ConcurrentDictionary<string, object?> _properties = new();

        protected T? Getter<T>([CallerMemberName] string? propertyName = null)
        {
            return (T)_properties.GetOrAdd(_id + propertyName, default(T));
        }

        protected void Setter<T>(T? value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(Getter<T>(propertyName), value))
            {
                _properties[_id + propertyName] = value;
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