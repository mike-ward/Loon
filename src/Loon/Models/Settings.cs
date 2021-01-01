using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Loon.Extensions;
using Loon.Interfaces;

namespace Loon.Models
{
    public sealed class Settings : NotifyPropertyChanged, ISettings
    {
        private string Profile { get; } = "loon";

        [JsonIgnore]
        public bool IsAuthenticated => AccessToken.IsPopulated() && AccessTokenSecret.IsPopulated();

        public string? AccessToken
        {
            get => Getter<string>();
            set
            {
                var tmp = IsAuthenticated;
                Setter(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? AccessTokenSecret
        {
            get => Getter<string>();
            set
            {
                var tmp = IsAuthenticated;
                Setter(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? ScreenName { get => Getter<string>(); set => Setter(value); }
        public bool HideProfileImages { get => Getter<bool>(); set => Setter(value); }
        public bool HideImages { get => Getter<bool>(); set => Setter(value); }
        public bool HideExtendedContent { get => Getter<bool>(); set => Setter(value); }
        public bool HideScreenName { get => Getter<bool>(); set => Setter(value); }
        public bool HidePossiblySensitive { get => Getter<bool>(); set => Setter(value); }
        public bool SpellCheck { get => Getter<bool>(); set => Setter(value); }
        public bool ShowInSystemTray { get => Getter<bool>(); set => Setter(value); }
        public bool AlwaysOnTop { get => Getter<bool>(); set => Setter(value); }
        public bool Donated { get => Getter<bool>(); set => Setter(value); }
        public bool ApplyGrayscaleShader { get => Getter<bool>(); set => Setter(value); }
        public double FontSize { get => Getter<double>(); set => Setter(value); }
        public bool UseLightTheme { get => Getter<bool>(); set => Setter(value); }
        public string? MyTweetColor { get => Getter<string>(); set => Setter(value); }
        public WindowLocation Location { get; set; } = new WindowLocation { X = 200, Y = 200, Width = 300, Height = 500 };

        [JsonIgnore]
        public string SettingsFilePath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            $"{Profile}.settings.txt");

        public void Load()
        {
            try
            {
                var json = File.ReadAllText(SettingsFilePath);
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