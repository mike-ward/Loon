using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Loon.Views.Content.Controls
{
    internal class ImageViewer : Window
    {
        public bool IsClosed { get; private set; }

        public ImageViewer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<ImageViewer, IImage?>(nameof(Source));

        public IImage? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public void HideWindow(object? sender, PointerPressedEventArgs e)
        {
            Hide();
            Source = null;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
    }
}