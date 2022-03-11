using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;

namespace Loon.Views.Content.AppSettings
{
    public class SettingsCheckboxes : UserControl
    {
        public ISystemState SystemState { get; init; }
        public bool         IsWindows   { get; } = OperatingSystem.IsWindows();

        public SettingsCheckboxes()
        {
            SystemState = new SystemState();
            AvaloniaXamlLoader.Load(this);
        }
    }
}