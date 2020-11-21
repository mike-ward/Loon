using Microsoft.Extensions.DependencyInjection;
using System;
using TweetX.Interfaces;
using TweetX.Models;
using TweetX.ViewModels;
using TweetX.ViewModels.Content.SignIn;
using TweetX.Views;

namespace TweetX.Services
{
    internal static class BootStrapper
    {
        private static ServiceProvider ServiceProvider { get; }

        static BootStrapper()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();

            // View Models
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<GetPinViewModel>();

            // Models
            services.AddSingleton<ISettings, Settings>();
        }

        public static object GetService(Type type)
        {
            return ServiceProvider.GetService(type
                ?? throw new ArgumentNullException(nameof(type)))
                ?? throw new NotSupportedException(type.Name);
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>()
                ?? throw new NotSupportedException(typeof(T).Name);
        }
    }
}