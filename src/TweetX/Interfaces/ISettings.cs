using System.ComponentModel;
using TweetX.Models;

namespace TweetX.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        WindowLocation Location { get; set; }
        string? AccessToken { get; set; }
        string? AccessTokenSecret { get; set; }
        string? ScreenName { get; set; }

        void Load();

        void Save();
    }
}