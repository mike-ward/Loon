using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using TweetX.Extensions;
using TweetX.Interfaces;
using TweetX.Services;
using Twitter.Models;

namespace TweetX.Models
{
    public class Timeline : NotifyPropertyChanged
    {
        private bool pendingStatusesAvailable;
        private string? exceptionMessage;

        private bool inUpdate;
        private readonly double intervalInMinutes = 1.1;
        private readonly DispatcherTimer updateTimer;

        public string TimelineName { get; init; }
        public IEnumerable<Func<Timeline, ValueTask>> UpdateTasks { get; init; }

        public AvaloniaList<TwitterStatus> StatusCollection { get; } = new();
        public ISet<string> AlreadyAdded { get; } = new HashSet<string>(StringComparer.Ordinal);
        public ISet<TwitterStatus> PendingStatusCollection { get; } = new HashSet<TwitterStatus>();
        public bool IsScrolled { get; set; }
        public ISettings Settings { get; }
        public bool PendingStatusesAvailable { get => pendingStatusesAvailable; set => SetProperty(ref pendingStatusesAvailable, value); }
        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }

        public Timeline(string name, IEnumerable<Func<Timeline, ValueTask>> updateTasks, ISettings settings)
        {
            TimelineName = name;
            UpdateTasks = updateTasks;
            Settings = settings;

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(intervalInMinutes) };
            updateTimer.Tick += async (_, __) => await UpdateAsync().ConfigureAwait(false);

            Settings.PropertyChanged += Start;
        }

        private async ValueTask UpdateAsync()
        {
            try
            {
                if (inUpdate)
                {
                    TraceService.Message($"{TimelineName} inUpdate");
                    return;
                }

                inUpdate = true;
                ExceptionMessage = null;

                TraceService.Message($"{TimelineName}: Updating");

                foreach (var updateTask in UpdateTasks)
                {
                    await updateTask(this).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message($"{TimelineName}: ${ex.Message}");
                ExceptionMessage = $"{ex.Message}";
            }
            finally
            {
                inUpdate = false;
            }
        }

        private async void Start(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.IsEqualTo(nameof(Settings.AccessToken)))
            {
                if (Settings.AccessToken is not null)
                {
                    if (!updateTimer.IsEnabled)
                    {
                        updateTimer.Start();
                        await UpdateAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    Stop();
                }
            }
        }

        private void Stop()
        {
            updateTimer?.Stop();
            AlreadyAdded.Clear();
            StatusCollection.Clear();
        }
    }
}