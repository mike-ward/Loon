using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Models
{
    internal class Timeline : NotifyPropertyChanged
    {
        private          bool            inUpdate;
        private readonly DispatcherTimer updateTimer;

        public  string                                 TimelineName            { get; }
        public  ISettings                              Settings                { get; }
        public  AvaloniaList<TwitterStatus>            StatusCollection        { get; } = new();
        public  IEnumerable<Func<Timeline, ValueTask>> UpdateTasks             { get; }
        public  ISet<string>                           AlreadyAdded            { get; } = new HashSet<string>(StringComparer.Ordinal);
        public  ISet<TwitterStatus>                    PendingStatusCollection { get; } = new HashSet<TwitterStatus>();
        public  bool                                   IsScrolled              { get; set; }
        private string?                                exceptionMessage;

        public string? ExceptionMessage
        {
            get => exceptionMessage;
            set => SetProperty(ref exceptionMessage, value);
        }

        private bool pendingStatusAvailable;

        public bool PendingStatusesAvailable
        {
            get => pendingStatusAvailable;
            set => SetProperty(ref pendingStatusAvailable, value);
        }

        public Timeline(string name, double intervalInMinutes, IEnumerable<Func<Timeline, ValueTask>> updateTasks, ISettings settings)
        {
            TimelineName = name;
            UpdateTasks  = updateTasks;
            Settings     = settings;

            updateTimer      =  new DispatcherTimer { Interval = TimeSpan.FromMinutes(intervalInMinutes) };
            updateTimer.Tick += UpdateTimerTick;

            Settings.PropertyChanged += CheckAuthentication;
        }

        private async void UpdateTimerTick(object? sender, EventArgs e)
        {
            await UpdateAsync().ConfigureAwait(false);
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

                inUpdate         = true;
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
            if (e.PropertyName.IsEqualTo(nameof(ISettings.IsAuthenticated)))
            {
                if (Settings.IsAuthenticated)
                {
                    await Start().ConfigureAwait(false);
                }
                else
                {
                    await Stop().ConfigureAwait(false);
                }
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