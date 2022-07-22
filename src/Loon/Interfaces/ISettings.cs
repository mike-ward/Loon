using System.ComponentModel;
using Avalonia;
using Loon.Models;

namespace Loon.Interfaces
{
    public interface ISettings : INotifyPropertyChanged
    {
        bool                      IsAuthenticated       { get; }
        string?                   AccessToken           { get; set; }
        string?                   AccessTokenSecret     { get; set; }
        string?                   ScreenName            { get; set; }
        string?                   TranslateApiKey       { get; }
        bool                      HideProfileImages     { get; }
        bool                      HideImages            { get; }
        bool                      HideExtendedContent   { get; }
        bool                      HidePossiblySensitive { get; }
        bool                      HideTranslate         { get; }
        bool                      ShowInSystemTray      { get; set; }
        bool                      AlwaysOnTop           { get; set; }
        double                    FontSize              { get; set; }
        bool                      UseLightTheme         { get; }
        bool                      ShortLinks            { get; set; }
        bool                      ImagesAsLinks         { get; set; }
        bool                      Donated               { get; }
        string                    Zoom                  { get; set; }
        double                    ZoomFontSize          { get; }
        double                    ZoomProfileImageSize  { get; }
        Rect                      ZoomProfileImageRect  { get; }
        double                    ZoomImagePanelHeight  { get; }
        double                    ZoomImagePanelWidth   { get; }
        WindowLocation            Location              { get; set; }
        ObservableHashSet<string> HiddenImagesSet       { get; set; }

        void Load();

        void Save();
    }
}