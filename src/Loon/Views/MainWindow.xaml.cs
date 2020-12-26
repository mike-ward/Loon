using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.ViewModels;

namespace Loon.Views
{
    public class MainWindow : Window, IWindow
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            LoadCloseSetup();
            LinuxSetup();
        }

        private void LoadCloseSetup()
        {
            var vm = (MainWindowViewModel)DataContext!;
            vm.Load(this);
            Closing += delegate { vm.Save(this); };
        }

        private void LinuxSetup()
        {
            if (OperatingSystem.IsLinux())
            {
                // Linux won't allow us to extend into the non-client areas
                this.FindControl<TitleBar>("TitleBar").IsVisible = false;
                ExtendClientAreaToDecorationsHint = false;
                ExtendClientAreaTitleBarHeightHint = 0;
            }
        }
    }
}