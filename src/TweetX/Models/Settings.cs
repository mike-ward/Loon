using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TweetX.Interfaces;

namespace TweetX.Models
{
    internal sealed class Settings : ISettings
    {
        public WindowLocation Location { get; set; } = new WindowLocation { X = 200, Y = 200, Width = 300, Height = 500 };

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