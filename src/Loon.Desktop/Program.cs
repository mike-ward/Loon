using System;
using Avalonia;

namespace Loon.Desktop
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
               .StartWithClassicDesktopLifetime(args);
        }

        // Previewer requires this method
        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder
               .Configure<App>()
               .UsePlatformDetect()
               .LogToTrace();
        }
    }
}