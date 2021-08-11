using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Commands;
using Loon.Services;
using Loon.Views;

namespace Loon
{
    internal class App : Application
    {
        public static ServiceProvider ServiceProvider { get; } = new();
        public static AppCommands     Commands        { get; } = ServiceProvider.GetService<AppCommands>();
        public static Window          MainWindow      => ((IClassicDesktopStyleApplicationLifetime)Current.ApplicationLifetime).MainWindow;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Visual designer won't work otherwise
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime app)
            {
                app.MainWindow = ServiceProvider.GetService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public static string GetString(string name)
        {
            return Current.TryFindResource(name, out var value) && value is string val
                ? val
                : $"string resource not found: {name}";
        }
    }
}