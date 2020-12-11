using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Loon.Interfaces;

namespace Loon.Models
{
    public sealed class Settings : NotifyPropertyChanged, ISettings
    {
        private string? accessToken;
        private string? accessTokenSecret;
        private string? screenName;
        private bool hideProfileImages;
        private bool hideImages;
        private bool hideExtendedContent;
        private bool hideScreenName;
        private bool hidePossiblySensitive;
        private bool donated;
        private bool spellCheck;
        private bool showInSystemTray;
        private bool alwaysOnTop;
        private bool applyGrayscaleShader;
        private double fontSize = 12;
        private string theme = "dark";
        private string? myTweetColor;
        private string Profile { get; } = "loon";

        public string? AccessToken
        {
            get => accessToken;
            set
            {
                var tmp = IsAuthenticated;
                SetProperty(ref accessToken, value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? AccessTokenSecret
        {
            get => accessTokenSecret;
            set
            {
                var tmp = IsAuthenticated;
                SetProperty(ref accessTokenSecret, value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        [JsonIgnore]
        public bool IsAuthenticated =>
            !string.IsNullOrWhiteSpace(AccessToken) &&
            !string.IsNullOrWhiteSpace(AccessTokenSecret);

        public string? ScreenName { get => screenName; set => SetProperty(ref screenName, value); }
        public bool HideProfileImages { get => hideProfileImages; set => SetProperty(ref hideProfileImages, value); }
        public bool HideImages { get => hideImages; set => SetProperty(ref hideImages, value); }
        public bool HideExtendedContent { get => hideExtendedContent; set => SetProperty(ref hideExtendedContent, value); }
        public bool HideScreenName { get => hideScreenName; set => SetProperty(ref hideScreenName, value); }
        public bool HidePossiblySensitive { get => hidePossiblySensitive; set => SetProperty(ref hidePossiblySensitive, value); }
        public bool SpellCheck { get => spellCheck; set => SetProperty(ref spellCheck, value); }
        public bool ShowInSystemTray { get => showInSystemTray; set => SetProperty(ref showInSystemTray, value); }
        public bool AlwaysOnTop { get => alwaysOnTop; set => SetProperty(ref alwaysOnTop, value); }
        public bool Donated { get => donated; set => SetProperty(ref donated, value); }
        public bool ApplyGrayscaleShader { get => applyGrayscaleShader; set => SetProperty(ref applyGrayscaleShader, value); }
        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }
        public string Theme { get => theme; set => SetProperty(ref theme, value); }
        public string? MyTweetColor { get => myTweetColor; set => SetProperty(ref myTweetColor, value); }
        public WindowLocation Location { get; set; } = new WindowLocation { X = 200, Y = 200, Width = 300, Height = 500 };

        [JsonIgnore]
        public string SettingsFilePath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
            $"{Profile}.settings.txt");

        public void Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<Settings>(json)!;

                AccessToken = settings.AccessToken;
                AccessTokenSecret = settings.AccessTokenSecret;
                ScreenName = settings.ScreenName;
                HideProfileImages = settings.HideProfileImages;
                HideImages = settings.HideImages;
                HideExtendedContent = settings.HideExtendedContent;
                HideScreenName = settings.HideScreenName;
                HidePossiblySensitive = settings.HidePossiblySensitive;
                SpellCheck = settings.SpellCheck;
                ShowInSystemTray = settings.showInSystemTray;
                AlwaysOnTop = settings.alwaysOnTop;
                FontSize = settings.FontSize;
                Theme = settings.Theme;
                ApplyGrayscaleShader = settings.ApplyGrayscaleShader;
                MyTweetColor = settings.MyTweetColor;
                Donated = settings.Donated;
                Location = settings.Location;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize<Settings>(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsFilePath, json);
        }
    }
}