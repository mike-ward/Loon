using TweetX.Interfaces;
using TweetX.Models;

namespace TweetX.ViewModels.Content
{
    public class ContentViewModel : NotifyPropertyChanged
    {
        public ContentViewModel(ISettings settings)
        {
            settings.PropertyChanged += delegate
            {
                SignedIn = !string.IsNullOrEmpty(settings.AccessToken);
            };
        }

        private bool signedIn;

        public bool SignedIn { get => signedIn; set { SetProperty(ref signedIn, value); } }
    }
}