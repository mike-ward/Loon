using TweetX.Interfaces;
using TweetX.Models;

namespace TweetX.ViewModels.Content
{
    public class ContentViewModel : NotifyPropertyChanged
    {
        private bool signedIn;
        public bool SignedIn { get => signedIn; set { SetProperty(ref signedIn, value); } }

        public ContentViewModel(ISettings settings)
        {
            settings.PropertyChanged += delegate
            {
                SignedIn = settings.AccessToken is not null && settings.AccessTokenSecret is not null;
            };
        }
    }
}