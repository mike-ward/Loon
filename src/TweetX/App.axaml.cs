using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TweetX.Services;
using TweetX.Views;

namespace TweetX
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).MainWindow = BootStrapper.GetService<MainWindow>();
            base.OnFrameworkInitializationCompleted();
        }

        public static Window MainWindow
        {
            get { return ((IClassicDesktopStyleApplicationLifetime)Current.ApplicationLifetime).MainWindow; }
        }

        public static void Shutdown()
        {
            ((IClassicDesktopStyleApplicationLifetime)Current.ApplicationLifetime).Shutdown();
        }
    }
}