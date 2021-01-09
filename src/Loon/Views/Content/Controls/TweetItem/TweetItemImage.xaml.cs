using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemImage : UserControl
    {
        private volatile bool clearing;

        public bool Clearing
        {
            get { return clearing; }
            set { clearing = value; }
        }

        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void LoadMediaAsync(object? sender, EventArgs e)
        {
            try
            {
                Clearing = false;

                if (sender is Image image)
                {
                    image.Source = null;
                    await Task.Delay(300).ConfigureAwait(true);
                    if (Clearing) { return; }

                    var media = image.DataContext as Media;

                    if (media?.MediaUrl.Length > 0)
                    {
                        var imageSource = await ImageService.GetImageAsync(media.MediaUrl, () => Clearing).ConfigureAwait(true);
                        if (!Clearing && imageSource is not null) { image.Source = imageSource; }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        public void OpenInViewer(object? sender, PointerPressedEventArgs e)
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