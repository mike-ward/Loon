using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TweetX.Interfaces;

namespace TweetX.Models
{
    internal sealed class Settings : NotifyPropertyChanged, ISettings
    {
        private string? accessToken;
        private string? accessTokenSecret;
        private string? screenName;

        public WindowLocation Location { get; set; } = new WindowLocation { X = 200, Y = 200, Width = 300, Height = 500 };
        public string? AccessToken { get => accessToken; set => SetProperty(ref accessToken, value); }
        public string? AccessTokenSecret { get => accessTokenSecret; set => SetProperty(ref accessTokenSecret, value); }
        public string? ScreenName { get => screenName; set => SetProperty(ref screenName, value); }

        private string Profile { get; } = "tweetx";

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

                Location = settings.Location;
                AccessToken = settings.AccessToken;
                AccessTokenSecret = settings.AccessTokenSecret;
                ScreenName = settings.ScreenName;
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