using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using TweetX.Extensions;
using TweetX.Interfaces;
using TweetX.Services;
using Twitter.Models;

namespace TweetX.Models
{
    internal class Timeline : NotifyPropertyChanged
    {
        private ISettings Settings { get; }

        private bool pendingStatusesAvailable;
        private string? exceptionMessage;

        private bool inUpdate;
        private readonly double intervalInMinutes = 1.1;
        private readonly DispatcherTimer updateTimer;
        private readonly string timelineName = "Home";
        private readonly List<Func<Timeline, ValueTask>> updateTasks = new();

        private ISet<string> AlreadyAdded { get; } = new HashSet<string>(StringComparer.Ordinal);
        private ObservableCollection<TwitterStatus> StatusCollection { get; } = new ObservableCollection<TwitterStatus>();
        public ISet<TwitterStatus> PendingStatusCollection { get; } = new HashSet<TwitterStatus>();
        public bool PendingStatusesAvailable { get => pendingStatusesAvailable; set => SetProperty(ref pendingStatusesAvailable, value); }

        public string? ExceptionMessage { get => exceptionMessage; set => SetProperty(ref exceptionMessage, value); }

        public Timeline(ISettings settings)
        {
            Settings = settings;

            updateTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(intervalInMinutes) };
            updateTimer.Tick += async (_, __) => await UpdateAsync().ConfigureAwait(false);

            Settings.PropertyChanged += OnAuthenticationChanged;
        }

        private async ValueTask UpdateAsync()
        {
            try
            {
                if (inUpdate)
                {
                    TraceService.Message($"{timelineName} inUpdate");
                    return;
                }

                inUpdate = true;
                ExceptionMessage = null;

                TraceService.Message($"{timelineName}: Updating");

                foreach (var updateTask in updateTasks)
                {
                    await updateTask(this).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message($"{timelineName}: ${ex.Message}");
                ExceptionMessage = $"{ex.Message}";
            }
            finally
            {
                inUpdate = false;
            }
        }

        private async void OnAuthenticationChanged(object? sender, PropertyChangedEventArgs e)
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

        public void AddUpdateTask(Func<Timeline, ValueTask> task)
        {
            updateTasks.Add(task);
        }
    }
}