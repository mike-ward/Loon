﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

// ReSharper disable MemberCanBePrivate.Global

namespace Loon.Models
{
    public delegate ValueTask UpdateTaskFunc(Timeline timeline);

    public sealed class Timeline : NotifyPropertyChanged
    {
        private          bool            inUpdate;
        private readonly DispatcherTimer updateTimer;

        public string                      TimelineName     { get; }
        public ISettings                   Settings         { get; }
        public AvaloniaList<TwitterStatus> StatusCollection { get; } = new();
        public IEnumerable<UpdateTaskFunc> UpdateTasks      { get; }
        public ISet<string>                AlreadyAdded     { get; } = new HashSet<string>(StringComparer.Ordinal);

        public ISet<TwitterStatus> PendingStatusCollection { get; } =
            new SortedSet<TwitterStatus>(new TwitterStatusSortComparer());

        public  bool    IsScrolled { get; set; }
        private string? exceptionMessage;

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

        public Timeline(string name, double intervalInMinutes, IEnumerable<UpdateTaskFunc> updateTasks,
            ISettings settings)
        {
            TimelineName = name;
            UpdateTasks  = updateTasks;
            Settings     = settings;

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(intervalInMinutes)
            };

            updateTimer.Tick         += UpdateTimerTick;
            Settings.PropertyChanged += CheckAuthentication;
        }

        private void UpdateTimerTick(object? sender, EventArgs e)
        {
            var unused = UpdateAsync();
        }

        public async ValueTask UpdateAsync()
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

                foreach (var updateTask in UpdateTasks) await updateTask(this).ConfigureAwait(true);
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

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void CheckAuthentication(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName.IsNotEqualTo(nameof(ISettings.IsAuthenticated))) return;

                if (Settings.IsAuthenticated)
                    await Start();
                else
                    await Stop();
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        private async Task Start()
        {
            if (!updateTimer.IsEnabled)
                await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        updateTimer.Start();
                        await UpdateAsync().ConfigureAwait(false);
                    })
                    .ConfigureAwait(false);
        }

        private Task Stop()
        {
            return Dispatcher.UIThread.InvokeAsync(() =>
            {
                updateTimer.Stop();
                AlreadyAdded.Clear();
                StatusCollection.Clear();
            });
        }
    }
}