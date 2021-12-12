using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Loon.Views.Content.Controls
{
    public class ImageViewer : Window
    {
        public ImageViewer()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<ImageViewer, IImage?>(nameof(Source));

        public IImage? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public void HideWindow(object? sender, PointerPressedEventArgs e)
        {
            Close();
        }
    }
}