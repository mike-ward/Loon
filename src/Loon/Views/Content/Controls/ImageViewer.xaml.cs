using System;
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
            Source = null;
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            MoveAndResize(e.Delta.Y > 0);
            e.Handled = true;
        }

        private void MoveAndResize(bool larger)
        {
            var deltaWidth  = Width * 0.05;
            var deltaHeight = Height * 0.05;
            var deltaX      = (int)Math.Round(deltaWidth / 2);
            var deltaY      = (int)Math.Round(deltaHeight / 2);

            var w = larger
                ? +deltaWidth
                : -deltaWidth;

            var h = larger
                ? +deltaHeight
                : -deltaHeight;

            var px = larger
                ? -deltaX
                : +deltaX;

            var py = larger
                ? -deltaY
                : +deltaY;

            Position =  new PixelPoint(Position.X + px, Position.Y + py);
            Width    += w;
            Height   += h;
        }
    }
}