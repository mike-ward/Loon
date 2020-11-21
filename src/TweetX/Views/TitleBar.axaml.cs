using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views
{
    internal class TitleBar : UserControl
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
            MessageBox.Show(App.MainWindow, "Hello World!", "TweetX", MessageBox.MessageBoxButtons.OkCancel);
            e.Handled = true;
            //App.MainWindow.WindowState = WindowState.Minimized;
        }

        public void BeginMoveDrag(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            App.MainWindow.BeginMoveDrag(e);
        }
    }
}