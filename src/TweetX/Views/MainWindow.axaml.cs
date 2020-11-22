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

            var vm = (MainWindowViewModel)DataContext!;
            vm.Load(this);
            Closing += delegate { vm.Save(this); };
        }
    }
}