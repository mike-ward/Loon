using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views
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

        public void BeginMoveDrag(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            App.MainWindow.BeginMoveDrag(e);
        }
    }
}