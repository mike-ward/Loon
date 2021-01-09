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
        public bool IsAuthenticated => AccessToken.IsNotVacant() && AccessTokenSecret.IsNotVacant();

        public string? AccessToken
        {
            get => Getter(default(string));
            set
            {
                var tmp = IsAuthenticated;
                Setter(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? AccessTokenSecret
        {
            get => Getter(default(string));
            set
            {
                var tmp = IsAuthenticated;
                Setter(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public bool AlwaysOnTop { get => Getter(false); set => Setter(value); }
        public bool ApplyGrayscaleShader { get => Getter(false); set => Setter(value); }
        public bool Donated { get => Getter(false); set => Setter(value); }
        public bool HideExtendedContent { get => Getter(false); set => Setter(value); }
        public bool HideImages { get => Getter(false); set => Setter(value); }
        public bool HidePossiblySensitive { get => Getter(false); set => Setter(value); }
        public bool HideProfileImages { get => Getter(false); set => Setter(value); }
        public bool HideScreenName { get => Getter(false); set => Setter(value); }
        public bool HideTranslate { get => Getter(false); set => Setter(value); }
        public bool ShowInSystemTray { get => Getter(false); set => Setter(value); }
        public bool UseLightTheme { get => Getter(false); set => Setter(value); }
        public double FontSize { get => Getter(12d); set => Setter(value); }
        public string? ScreenName { get => Getter(default(string)); set => Setter(value); }
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