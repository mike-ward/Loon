using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Loon.Extensions;
using Loon.Views.Content.Controls;
using Twitter.Models;

namespace Loon.Services
{
    internal static class ImageViewerService
    {
        private static ImageViewer? imageViewer;
        private static Process?     videoPlayerProcess;
        private static string?      previousVideoUrl;

        public static void OpenInViewer(Image image)
        {
            KillImageViewer();
            async void ActionOpenViewer() => await OpenInViewerTask(image);
            Dispatcher.UIThread.Post(ActionOpenViewer);
        }

        public static void KillImageViewer()
        {
            videoPlayerProcess?.Kill();
            videoPlayerProcess?.Close();
            videoPlayerProcess = null;
            imageViewer?.Close();
        }

        public static string VideoUrl(Media? media)
        {
            return media
              ?.VideoInfo
              ?.Variants
              ?.Select(variant => variant.Url)
               .First() ?? string.Empty;
        }

        private static async Task OpenInViewerTask(Image image)
        {
            var videoUrl = VideoUrl(image.DataContext as Media);

            if (videoUrl.IsNotNullOrWhiteSpace())
            {
                if (videoUrl.IsEqualTo(previousVideoUrl))
                {
                    previousVideoUrl = string.Empty;
                }
                else
                {
                    videoPlayerProcess = await PlayVideo(videoUrl);
                }
            }
            else if (image.Source is not null)
            {
                previousVideoUrl = string.Empty;
                imageViewer      = new ImageViewer(image.Source);
                imageViewer.Show(App.MainWindow);
            }
        }

        private static async Task<Process?> PlayVideo(string videoUrl)
        {
            var pi = new ProcessStartInfo
            {
                FileName       = "vlc",
                Arguments      = $"--one-instance --quiet --no-osd --no-skins2-taskbar {videoUrl}",
                CreateNoWindow = true
            };
            try
            {
                var process = Process.Start(pi);
                previousVideoUrl = videoUrl;
                return process;
            }
            catch (Win32Exception)
            {
                await MessageBox.Show(App.GetString("install-vlc"), MessageBox.MessageBoxButtons.Ok);
            }
            catch (InvalidOperationException)
            {
                await MessageBox.Show(App.GetString("install-vlc"), MessageBox.MessageBoxButtons.Ok);
            }

            return null;
        }
    }
}