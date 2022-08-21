using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Loon.Views.Content.Controls
{
    public sealed class ImageViewer : Window
    {
        private Image? imageControl { get; }

        public ImageViewer() // not used, but required by XAML
        {
            imageControl = null!;
        }

        public ImageViewer(IImage image)
        {
            Source = image;
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
            var zoomFactor = larger
                ? 0.1
                : -0.1;

            var width  = imageControl!.Width + Width * zoomFactor;
            var height = imageControl.Height + Height * zoomFactor;

            if (width < 100 || height < 100) return;
            imageControl.Width  = width;
            imageControl.Height = height;
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    ResizeImage(false);
                    e.Handled = true;
                    break;
                case Key.Up:
                    ResizeImage(true);
                    e.Handled = true;
                    break;
            }
        }
    }
}