using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Loon.Extensions;
using Loon.Interfaces;

namespace Loon.Models
{
    internal sealed class Settings : NotifyPropertyChanged, ISettings
    {
        private string Profile { get; } = "loon";

        [JsonIgnore]
        private bool isAuthenticated;

        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set => SetProperty(ref isAuthenticated, value);
        }

        private bool GetCheckAuthenticated()
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
                IsAuthenticated = GetCheckAuthenticated();
            }
        }

        private string? accessTokenSecret;

        public string? AccessTokenSecret
        {
            get => accessTokenSecret;
            set
            {
                SetProperty(ref accessTokenSecret, value);
                IsAuthenticated = GetCheckAuthenticated();
            }
        }

        private bool alwaysOnTop;

        public bool AlwaysOnTop
        {
            get => alwaysOnTop;
            set => SetProperty(ref alwaysOnTop, value);
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

        private bool hideProfileImages;

        public bool HideProfileImages
        {
            get => hideProfileImages;
            set => SetProperty(ref hideProfileImages, value);
        }

        private bool hideScreenName;

        public bool HideScreenName
        {
            get => hideScreenName;
            set => SetProperty(ref hideScreenName, value);
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

        private bool useLightTheme;

        public bool UseLightTheme
        {
            get => useLightTheme;
            set => SetProperty(ref useLightTheme, value);
        }

        private double fontSize = 12;

        public double FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }

        private string? screenName;

        public string? ScreenName
        {
            get => screenName;
            set => SetProperty(ref screenName, value);
        }

        public WindowLocation Location { get; set; } = new() { X = 200, Y = 200, Width = 300, Height = 500 };

        public string? TranslateApiKey { get; set; }

        [JsonIgnore]
        public string SettingsFilePath => Path.Combine(
            AppContext.BaseDirectory,
            $"{Profile}.settings.txt");

        public void Load()
        {
            try
            {
                var json     = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json)!;
                settings.CopyPropertiesTo(this);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
    }
}