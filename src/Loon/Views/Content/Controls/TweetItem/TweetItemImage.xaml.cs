using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemImage : UserControl
    {
        private volatile uint nextId;

        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

#pragma warning disable RCS1213 // (used in XAML) Remove unused member declaration.
#pragma warning disable S1144 // (used in XAML) Unused private types or members should be removed

        private async void LoadMediaAsync(object? sender, EventArgs e)
        {
            if (sender is Image image)
            {
                try
                {
                    if (image.DataContext is Media media &&
                        media?.MediaUrl.Length > 0)
                    {
                        var id = Interlocked.Increment(ref nextId);

                        var source = await ImageService
                            .GetImageAsync(media.MediaUrl, () => nextId)
                            .ConfigureAwait(true);

                        if (id == nextId)
                        {
                            image.Source = source;
                        }
                    }
                    else
                    {
                        image.Source = null;
                    }
                }
                catch (Exception ex)
                {
                    image.Source = null;
                    TraceService.Message(ex.Message);
                }
            }
        }

        private void OpenInViewer(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed &&
                sender is Grid grid &&
                grid.Children[0] is Image image)
            {
                ImageService.OpenInViewer(image);
            }
        }
    }
}