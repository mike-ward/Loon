using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Loon.Interfaces;
using Loon.ViewModels;

namespace Loon.Views
{
    public sealed class MainWindow : Window, IWindow
    {
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

        public MainWindow()
        {
            DataContext = App.ServiceProvider.GetService<MainWindowViewModel>();
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            IfWindowsSetup();
            ViewModel.Load(this);

            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            ViewModel.SetWindowLocation(this);
        }

        private void IfWindowsSetup()
        {
            if (OperatingSystem.IsWindows())
            {
                var border = (Border)Content!;
                border.BorderBrush       = Application.Current!.FindResource("ThemeBorderLowBrush") as SolidColorBrush ?? Brushes.Black;
                border.UseLayoutRounding = false;
                border.BorderThickness   = new Thickness(0.25);

                var grid     = (Grid)border.Child!;
                var titleBar = (TitleBar)grid.Children[0];
                titleBar.IsVisible                 = true;
                ExtendClientAreaToDecorationsHint  = true;
                ExtendClientAreaTitleBarHeightHint = 1;
            }
        }
    }
}