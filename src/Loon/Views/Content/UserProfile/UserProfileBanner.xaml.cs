using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Extensions;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileBanner : UserControl
    {
        public static readonly double BannerHeight = 150;

        public UserProfileBanner()
        {
            AvaloniaXamlLoader.Load(this);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        public async void LoadImageAsync(object? sender, EventArgs e)
        {
            try
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

                            var uri = which switch
                            {
                                "profile" => user.ProfileImageUrlBigger,
                                "banner"  => user.ProfileBannerUrlSmall,
                                _         => null
                            };

                            if (uri.IsNotNullOrWhiteSpace())
                            {
                                image.Source = await ImageService.GetImageAsync(uri!, CancellationToken.None).ConfigureAwait(true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceService.Message(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
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