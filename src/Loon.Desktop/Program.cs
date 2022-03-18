﻿using System;
using Avalonia;

namespace Loon.Desktop
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            AppBuilder
               .Configure<App>()
               .UsePlatformDetect()
               .LogToTrace()
               .StartWithClassicDesktopLifetime(args);
        }
    }
}