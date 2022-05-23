using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Loon.Commands;
using Loon.Interfaces;
using Loon.Services;
using Loon.ViewModels;
using Loon.Views;
using Loon.Views.Content;

namespace Loon
{
    public sealed class App : Application
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
            switch (ApplicationLifetime)
            {
                case IClassicDesktopStyleApplicationLifetime desktop:
                    // Visual designer won't work otherwise
                    RuntimeHelpers.RunClassConstructor(typeof(LongUrlService).TypeHandle);
                    desktop.MainWindow = ServiceProvider.GetService<MainWindow>();
                    break;
                case ISingleViewApplicationLifetime singleViewPlatform:
                    Settings.Load();
                    DataContext                 = ServiceProvider.GetService<MainWindowViewModel>();
                    singleViewPlatform.MainView = new AppView();
                    break;
                default:
                    throw new ApplicationException($"Unrecognized ApplicationLifeTime ({ApplicationLifetime})");
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