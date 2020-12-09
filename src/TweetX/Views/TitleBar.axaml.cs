using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views
{
    public class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void CloseApp(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            App.Shutdown();
        }

        public void MinimizeApp(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            e.Handled = true;
            App.MainWindow.WindowState = WindowState.Minimized;
        }

        public void BeginMoveDrag(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            App.MainWindow.BeginMoveDrag(e);
        }
    }
}