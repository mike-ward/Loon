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
            Resize(e.Delta.Y > 0);
        }

        private void Resize(bool delta)
        {
            var deltaWidth  = Width * 0.05;
            var deltaHeight = Height * 0.05;
            var deltaX      = (int)Math.Round(deltaWidth / 2);
            var deltaY      = (int)Math.Round(deltaHeight / 2);

            var w = delta
                ? +deltaWidth
                : -deltaWidth;

            var h = delta
                ? +deltaHeight
                : -deltaHeight;

            var px = delta
                ? -deltaX
                : +deltaX;

            var py = delta
                ? -deltaY
                : +deltaY;

            Position =  new PixelPoint(Position.X + px, Position.Y + py);
            Width    += w;
            Height   += h;
        }
    }
}