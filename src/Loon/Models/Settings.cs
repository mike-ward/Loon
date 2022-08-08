using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia;
using Loon.Extensions;
using Loon.Interfaces;

namespace Loon.Models
{
    internal sealed class Settings : NotifyPropertyChanged, ISettings
    {
        private string Profile { get; } = "loon";

        private bool isAuthenticated;

        [JsonIgnore]
        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set => SetProperty(ref isAuthenticated, value);
        }

        private bool IsAuthenticatedInternal()
        {
            return AccessToken.IsNotNullOrWhiteSpace() && AccessTokenSecret.IsNotNullOrWhiteSpace();
        }

        private string? accessToken;

        public string? AccessToken
        {
            get => accessToken;
            set
            {
                SetProperty(ref accessToken, value);
                IsAuthenticated = IsAuthenticatedInternal();
            }
        }

        private string? accessTokenSecret;

        public string? AccessTokenSecret
        {
            get => accessTokenSecret;
            set
            {
                SetProperty(ref accessTokenSecret, value);
                IsAuthenticated = IsAuthenticatedInternal();
            }
        }

        private bool alwaysOnTop;

        public bool AlwaysOnTop
        {
            get => alwaysOnTop;
            set => SetProperty(ref alwaysOnTop, value);
        }

        private bool imagesAsLinks;

        public bool ImagesAsLinks
        {
            get => imagesAsLinks;
            set => SetProperty(ref imagesAsLinks, value);
        }

        private bool donated;

        public bool Donated
        {
            get => donated;
            set => SetProperty(ref donated, value);
        }

        private bool hideExtendedContent;

        public bool HideExtendedContent
        {
            get => hideExtendedContent;
            set => SetProperty(ref hideExtendedContent, value);
        }

        private bool hideImages;

        public bool HideImages
        {
            get => hideImages;
            set => SetProperty(ref hideImages, value);
        }

        private bool hidePossiblySensitive;

        public bool HidePossiblySensitive
        {
            get => hidePossiblySensitive;
            set => SetProperty(ref hidePossiblySensitive, value);
        }

        private bool hideNameInTitleBar;

        public bool HideNameInTitleBar
        {
            get => hideNameInTitleBar;
            set
            {
                SetProperty(ref hideNameInTitleBar, value);
                OnPropertyChanged(nameof(ScreenName));
            }
        }

        private bool hideProfileImages;

        public bool HideProfileImages
        {
            get => hideProfileImages;
            set => SetProperty(ref hideProfileImages, value);
        }

        private bool hideTranslate;

        public bool HideTranslate
        {
            get => hideTranslate;
            set => SetProperty(ref hideTranslate, value);
        }

        private bool showInSystemTray;

        public bool ShowInSystemTray
        {
            get => showInSystemTray;
            set => SetProperty(ref showInSystemTray, value);
        }

        private bool shortLinks;

        public bool ShortLinks
        {
            get => shortLinks;
            set => SetProperty(ref shortLinks, value);
        }

        private double fontSize = 12;

        public double FontSize
        {
            get => fontSize;
            set
            {
                if (value is <= 5 or >= 40) return;
                SetProperty(ref fontSize, value);
                UpdateZoomProperties();
            }
        }

        // Zoom stuff
        // ----------------------------------------

        public const string Zoom100Percent = "100";
        public const string Zoom150Percent = "150";
        public const string Zoom200Percent = "200";

        private string zoom = Zoom100Percent;

        public string Zoom
        {
            get => zoom;
            set
            {
                SetProperty(ref zoom, value);
                UpdateZoomProperties();
            }
        }

        private void UpdateZoomProperties()
        {
            OnPropertyChanged(nameof(ZoomFontSize));
            OnPropertyChanged(nameof(ZoomProfileImageSize));
            OnPropertyChanged(nameof(ZoomProfileImageRect));
            OnPropertyChanged(nameof(ZoomImagePanelHeight));
            OnPropertyChanged(nameof(ZoomImagePanelWidth));
            OnPropertyChanged(nameof(ZoomIs100Percent));
            OnPropertyChanged(nameof(ZoomIs150Percent));
            OnPropertyChanged(nameof(ZoomIs200Percent));
        }

        public bool ZoomIs100Percent => Zoom.IsEqualTo(Zoom100Percent);
        public bool ZoomIs150Percent => Zoom.IsEqualTo(Zoom150Percent);
        public bool ZoomIs200Percent => Zoom.IsEqualTo(Zoom200Percent);

        private double ZoomFactor => Zoom switch
        {
            Zoom150Percent => 1.5,
            Zoom200Percent => 2.0,
            _              => 1.0
        };

        public double ZoomFontSize         => FontSize * ZoomFactor;
        public double ZoomProfileImageSize => Constants.ImageProfileSize * ZoomFactor;
        public Rect   ZoomProfileImageRect => new(0, 0, ZoomProfileImageSize, ZoomProfileImageSize);
        public double ZoomImagePanelHeight => Constants.ImagePanelHeight * ZoomFactor;
        public double ZoomImagePanelWidth  => Constants.ImagePanelWidth * ZoomFactor;

        // ----------------------------------------
        // End Zoom stuff

        private string? screenName;

        public string? ScreenName
        {
            get => screenName;
            set => SetProperty(ref screenName, value);
        }

        public WindowLocation Location { get; set; } = new()
        {
            X      = 200,
            Y      = 200,
            Width  = 300,
            Height = 500
        };

        public string? TranslateApiKey { get; set; }

        public ObservableHashSet<string> HiddenImagesSet { get; set; } = new();

        public void Load()
        {
            try
            {
                Console.WriteLine(SettingsFilePath);
                var json     = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json)!;
                settings.CopyPropertiesTo(this);

                void OnHiddenImageSetOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(HiddenImagesSet));
                HiddenImagesSet.CollectionChanged -= OnHiddenImageSetOnCollectionChanged;
                HiddenImagesSet.CollectionChanged += OnHiddenImageSetOnCollectionChanged;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void Save()
        {
            var directory = new FileInfo(SettingsFilePath).Directory;
            if (directory!.Exists is false) directory.Create();

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }

        private string? cachedSettingsFilePath;

        [JsonIgnore]
        public string SettingsFilePath
        {
            get
            {
                return cachedSettingsFilePath ??= Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    $"{Profile}.settings.txt");
            }
        }
    }
}