using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Extensions;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    internal class UserProfileBanner : UserControl
    {
        public static readonly double BannerHeight = 150;

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
            if (sender is Image image)
            {
                try
                {
                    image.Source = null;

                    if (image.DataContext is User user &&
                        image.Tag is string which)
                    {
                        CollapseHeightIfNoBanner(user);

                        var uri = which switch {
                            "profile" => user.ProfileImageUrlBigger,
                            "banner"  => user.ProfileBannerUrlSmall,
                            _         => null
                        };

                        if (uri.IsNotNullOrWhiteSpace())
                        {
                            image.Source = await ImageService.GetImageAsync(uri!).ConfigureAwait(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }
        }

        private void CollapseHeightIfNoBanner(User user)
        {
            var grid = this.FindDescendantOfType<Grid>();

            if (grid is not null)
            {
                grid.Height = user.ProfileBannerUrlSmall is not null
                    ? BannerHeight
                    : double.NaN;
            }
        }
    }
}