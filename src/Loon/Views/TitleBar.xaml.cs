using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Loon.Views
{
    public sealed class TitleBar : UserControl
    {
        public TitleBar()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void BeginMoveDrag(object? sender, PointerPressedEventArgs e)
        {
            App.MainWindow.BeginMoveDrag(e);
        }
    }
}