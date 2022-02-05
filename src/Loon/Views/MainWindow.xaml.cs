﻿using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.ViewModels;

namespace Loon.Views
{
    public class MainWindow : Window, IWindow
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
            ViewModel.SetWindowLocation(this);
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