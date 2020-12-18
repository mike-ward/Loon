using System;
using Loon.Interfaces;
using Loon.Models;
using Loon.ViewModels;
using Loon.ViewModels.Content;
using Loon.ViewModels.Content.Timelines;
using Loon.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Loon.Services
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
            services.AddTransient<HomeTimelineViewModel>();
            services.AddTransient<LikesTimelineViewModel>();
            services.AddTransient<UserProfileTimelineViewModel>();

            // Models
            services.AddSingleton<ISettings, Settings>();

            // Services
            services.AddSingleton<ITwitterService, TwitterService>();
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