using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Interfaces;
using Loon.ViewModels;

namespace Loon.Views
{
    internal class MainWindow : Window, IWindow
    {
        // ReSharper disable once ConvertToConstant.Global (needed to bind in XAML)
        public static readonly string TitleBarName = "TitleBar";

        public MainWindow()
        {
            DataContext = App.ServiceProvider.GetService<MainWindowViewModel>();
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ((MainWindowViewModel)DataContext!).Load(this);
            LinuxSetup();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void LinuxSetup()
        {
            if (OperatingSystem.IsLinux())
            {
                // On Linux, can't extend into non-client areas
                this.FindControl<TitleBar>(TitleBarName).IsVisible = false;
                ExtendClientAreaToDecorationsHint                  = false;
                ExtendClientAreaTitleBarHeightHint                 = 0;
            }
        }
    }
}