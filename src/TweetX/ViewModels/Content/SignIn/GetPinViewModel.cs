using TweetX.Interfaces;
using TweetX.Models;

namespace TweetX.ViewModels.Content.SignIn
{
    internal class GetPinViewModel : NotifiyPropertyChanged
    {
        private string? pin;

        public ISettings Settings { get; }
        public string? Pin { get => pin; set => SetProperty(ref pin, value); }

        public GetPinViewModel(ISettings settings)
        {
            Settings = settings;
        }
    }
}