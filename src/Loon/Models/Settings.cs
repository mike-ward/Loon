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
            get => GetProp<string>();
            set
            {
                var tmp = IsAuthenticated;
                SetProp(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? AccessTokenSecret
        {
            get => GetProp<string>();
            set
            {
                var tmp = IsAuthenticated;
                SetProp(value);
                if (IsAuthenticated != tmp) { OnPropertyChanged(nameof(IsAuthenticated)); }
            }
        }

        public string? ScreenName { get => GetProp<string>(); set => SetProp(value); }
        public bool HideProfileImages { get => GetProp<bool>(); set => SetProp(value); }
        public bool HideImages { get => GetProp<bool>(); set => SetProp(value); }
        public bool HideExtendedContent { get => GetProp<bool>(); set => SetProp(value); }
        public bool HideScreenName { get => GetProp<bool>(); set => SetProp(value); }
        public bool HidePossiblySensitive { get => GetProp<bool>(); set => SetProp(value); }
        public bool SpellCheck { get => GetProp<bool>(); set => SetProp(value); }
        public bool ShowInSystemTray { get => GetProp<bool>(); set => SetProp(value); }
        public bool AlwaysOnTop { get => GetProp<bool>(); set => SetProp(value); }
        public bool Donated { get => GetProp<bool>(); set => SetProp(value); }
        public bool ApplyGrayscaleShader { get => GetProp<bool>(); set => SetProp(value); }
        public double FontSize { get => GetProp<double>(); set => SetProp(value); }
        public bool UseLightTheme { get => GetProp<bool>(); set => SetProp(value); }
        public string? MyTweetColor { get => GetProp<string>(); set => SetProp(value); }
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