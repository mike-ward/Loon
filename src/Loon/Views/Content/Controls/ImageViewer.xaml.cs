using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Loon.Views.Content.Controls
{
    public sealed class ImageViewer : Window
    {
        private Image imageControl { get; }

        public ImageViewer()
        {
            AvaloniaXamlLoader.Load(this);
            imageControl = this.FindControl<Image>("imageControl");
        }

        public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<ImageViewer, IImage?>(nameof(Source));

        public IImage? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public void HideWindow(object? sender, PointerReleasedEventArgs e)
        {
            Close();
            Source = null;
        }

        public void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            ResizeImage(e.Delta.Y > 0);
            e.Handled = true;
        }

        private void ResizeImage(bool larger)
        {
            var direction = larger
                ? 1
                : -1;

            imageControl.Width  += Width * 0.05 * direction;
            imageControl.Height += Height * 0.05 * direction;
        }
    }
}