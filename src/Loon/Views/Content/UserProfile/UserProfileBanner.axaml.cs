using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileBanner : UserControl
    {
        public UserProfileBanner()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void LoadImageAsync(object? sender, EventArgs e)
        {
            try
            {
                if (sender is Image image)
                {
                    image.Source = null;

                    if (image.DataContext is User user && image.Tag is string which)
                    {
                        var uri = which switch
                        {
                            "profile" => user.ProfileImageUrlBigger,
                            "banner" => user.ProfileBannerUrl,
                            _ => null
                        };

                        if (uri is not null)
                        {
                            image.Source = await ImageService.GetImageAsync(uri, () => false).ConfigureAwait(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}