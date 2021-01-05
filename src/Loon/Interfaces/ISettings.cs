using System.ComponentModel;
using Loon.Models;

namespace Loon.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        bool IsAuthenticated { get; }
        string? AccessToken { get; set; }
        string? AccessTokenSecret { get; set; }
        string? ScreenName { get; set; }
        bool HideProfileImages { get; }
        bool HideImages { get; }
        bool HideExtendedContent { get; }
        bool HideScreenName { get; }
        bool HidePossiblySensitive { get; }
        bool HideTranslate { get; }
        bool ShowInSystemTray { get; set; }
        bool AlwaysOnTop { get; set; }
        double FontSize { get; set; }
        bool UseLightTheme { get; }
        bool ApplyGrayscaleShader { get; set; }
        bool Donated { get; }
        WindowLocation Location { get; set; }

        void Load();

        void Save();
    }
}