using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Loon.Commands;
using Loon.Interfaces;
using Loon.Services;
using Loon.Views;

namespace Loon
{
    public class App : Application
    {
        public static ServiceProvider ServiceProvider { get; } = new();
        public static AppCommands     Commands        { get; } = ServiceProvider.GetService<AppCommands>();
        public static ISettings       Settings        { get; } = ServiceProvider.GetService<ISettings>();
        public static Window          MainWindow      => ((IClassicDesktopStyleApplicationLifetime)Current!.ApplicationLifetime!).MainWindow;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Visual designer won't work otherwise
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app)
            {
                RuntimeHelpers.RunClassConstructor(typeof(LongUrlService).TypeHandle);
                app.MainWindow = ServiceProvider.GetService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static string GetString(string name)
        {
            return Current!.TryFindResource(name, out var value) && value is string val
                ? val
                : $"string resource not found: {name}";
        }
    }
}