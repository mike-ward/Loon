using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Interfaces;
using Loon.Services;
using Loon.ViewModels;

namespace Loon.Views
{
    public class MainWindow : Window, IWindow
    {
        public static readonly string TitleBarName = "TitleBar";

        public MainWindow()
        {
            DataContext = Bootstrapper.ServiceProvider.GetService<MainWindowViewModel>();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ((MainWindowViewModel)DataContext!).Load(this);
            LinuxSetup();
        }

        private void LinuxSetup()
        {
            if (OperatingSystem.IsLinux())
            {
                // On Linux, can't extend into non-client areas
                this.FindControl<TitleBar>(TitleBarName).IsVisible = false;
                ExtendClientAreaToDecorationsHint = false;
                ExtendClientAreaTitleBarHeightHint = 0;
            }
        }
    }
}