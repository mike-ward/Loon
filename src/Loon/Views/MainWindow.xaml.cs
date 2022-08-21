using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.ViewModels;
#if DEBUG
using Avalonia;
#endif

namespace Loon.Views
{
    public sealed class MainWindow : Window, IWindow
    {
        // ReSharper disable once ConvertToConstant.Global (needed to bind in XAML)
        public static readonly string              TitleBarName = "TitleBar";
        private                MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

        public MainWindow()
        {
            DataContext = App.ServiceProvider.GetService<MainWindowViewModel>();
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel.Load(this);

            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            LinuxSetup();
            MacOsSetup();
            ViewModel.SetWindowLocation(this);
        }

        private void LinuxSetup()
        {
            if (OperatingSystem.IsLinux())
            {
                // On Linux, can't extend into non-client areas
                HideCustomTitleBar();
            }
        }

        private void MacOsSetup()
        {
            if (OperatingSystem.IsMacOS())
            {
                HideCustomTitleBar();
            }
        }

        private void HideCustomTitleBar()
        {
            if (this.FindControl<TitleBar>(TitleBarName) is { } titlebar) titlebar.IsVisible = false;
            ExtendClientAreaToDecorationsHint  = false;
            ExtendClientAreaTitleBarHeightHint = 0;
        }
    }
}