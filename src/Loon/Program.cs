using System;
using Avalonia;

namespace Loon
{
    public static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // BuildAvaloniaApp() method require for Rider Previewer to work.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder
                .Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
        }
    }
}