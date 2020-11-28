using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TweetX.Interfaces;
using TweetX.ViewModels;

namespace TweetX.Views
{
    internal class MainWindow : Window, IWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (OperatingSystem.IsLinux())
            {
                // ExtendClientAreaToDecorationsHint not working in Linux at this time.
                this.FindControl<TitleBar>("TitleBar").IsVisible = false;
                ExtendClientAreaToDecorationsHint = false;
                ExtendClientAreaTitleBarHeightHint = 0;
            }

            var vm = (MainWindowViewModel)DataContext!;
            vm.Load(this);
            Closing += delegate { vm.Save(this); };
        }
    }
}