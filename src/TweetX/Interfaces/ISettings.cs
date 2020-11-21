using TweetX.Models;

namespace TweetX.Interfaces
{
    public interface ISettings
    {
        WindowLocation Location { get; set; }

        void Load();

        void Save();
    }
}