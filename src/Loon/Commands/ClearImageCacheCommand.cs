﻿using System;
using System.Diagnostics.CodeAnalysis;
using Loon.Services;
using Loon.Views.Content.Controls;

namespace Loon.Commands
{
    public sealed class ClearImageCacheCommand : BaseCommand
    {
        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        public override async void Execute(object? parameter)
        {
            try
            {
                if (await MessageBox.Show(App.GetString("settings-sure"), MessageBox.MessageBoxButtons.YesNo) == MessageBox.MessageBoxResult.Yes)
                {
                    ImageFileCacheService.ClearCache();
                    ImageMemoryCacheService.ClearCache();
                    Console.Beep();
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}