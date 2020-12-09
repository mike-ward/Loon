using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Models
{
    public class Timeline : NotifyPropertyChanged
    {
        private string? exceptionMessage;
        private bool pendingStatusesAvailable;

        private bool inUpdate;
        private readonly DispatcherTimer updateTimer;

        public string TimelineName { get; }
        public ISettings Settings { get; }
        public AvaloniaList<TwitterStatus> StatusCollection { get; } = new();
        public IEnumerable<Func<Timeline, ValueTask>> UpdateTasks { get; }
        public ISet<string> AlreadyAdded { get; } = new HashSet<string>(StringComparer.Ordinal);
        public ISet<TwitterStatus> PendingStatusCollection { get; } = new HashSet<TwitterStatus>();
        public bool IsScrolled { get; set; }
        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }
        public bool PendingStatusesAvailable { get => pendingStatusesAvailable; set => SetProperty(ref pendingStatusesAvailable, value); }

        public Timeline(string name, double intervalInMinutes, IEnumerable<Func<Timeline, ValueTask>> updateTasks, ISettings settings)
        {
            TimelineName = name;
            UpdateTasks = updateTasks;
            Settings = settings;

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(intervalInMinutes) };
            updateTimer.Tick += async delegate { await UpdateAsync().ConfigureAwait(false); };

            Settings.PropertyChanged += CheckAuthentication;
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

        private async void CheckAuthentication(object? sender, PropertyChangedEventArgs e)
        {
            if (Settings.AccessToken is not null &&
                Settings.AccessTokenSecret is not null)
            {
                await Start().ConfigureAwait(false);
            }
            else
            {
                await Stop().ConfigureAwait(false);
            }
        }

        private async Task Start()
        {
            if (!updateTimer.IsEnabled)
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    updateTimer.Start();
                    await UpdateAsync().ConfigureAwait(false);
                })
                .ConfigureAwait(false);
            }
        }

        private Task Stop()
        {
            return Dispatcher.UIThread.InvokeAsync(() =>
            {
                updateTimer?.Stop();
                AlreadyAdded.Clear();
                StatusCollection.Clear();
            });
        }
    }
}