using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Loon.Models
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>([NotNullIfNotNull("newValue")] ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) return;
            field = newValue;
            OnPropertyChanged(propertyName!);
        }

        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> eventArgsCache = new();

        protected void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, eventArgsCache.GetOrAdd(propertyName!, name => new PropertyChangedEventArgs(name)));
        }
    }
}