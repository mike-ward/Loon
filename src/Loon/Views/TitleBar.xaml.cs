using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views
{
    public class TitleBar : UserControl
    {
        public TitleBar()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void BeginMoveDrag(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            App.MainWindow.BeginMoveDrag(e);
        }
    }
}