using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TweetX.Interfaces;
using TweetX.ViewModels;

namespace TweetX.Views
{
    internal class MainWindow : Window, IWindow
    {
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

        public MainWindow()
        {
            InitializeComponent();
            Closing += delegate { ViewModel.Close(this); };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ViewModel.Initialize(this);
        }
    }
}