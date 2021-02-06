using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemImage : UserControl
    {
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
            if (sender is Image image &&
                image.DataContext is Media media)
            {
                try
                {
                    image.Source = null;

                    var imageSource = await ImageService
                        .GetImageAsync(media.MediaUrl)
                        .ConfigureAwait(true);

                    if (image.Source is null) // check for overlapped request
                    {
                        image.Source = imageSource;
                    }
                }
                catch (Exception ex)
                {
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