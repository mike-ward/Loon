using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Twitter.Models;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemProfileImage : UserControl
    {
        public TweetItemProfileImage()
        {
            InitializeComponent();
            Initialized += OnInitialized;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnInitialized(object? sender, System.EventArgs e)
        {
            base.OnInitialized();
            try
            {
                var uri = ((TwitterStatus)DataContext!).User.ProfileImageUrl!;
                var bitmap = await GetImage(uri).ConfigureAwait(true);
                var image = this.FindControl<Image>("Image");
                image.Source = bitmap;
            }
            catch
            {
                // eat it for now
            }
        }

        private static async ValueTask<IImage> GetImage(string uri)
        {
            var http = new HttpClient();
            var response = await http.GetAsync(uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return new Bitmap(stream);
        }
    }
}