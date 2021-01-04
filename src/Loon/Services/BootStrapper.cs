using System;
using Loon.Commands;
using Loon.Interfaces;
using Loon.Models;
using Loon.ViewModels;
using Loon.ViewModels.Content;
using Loon.ViewModels.Content.Timelines;
using Loon.ViewModels.Content.UserProfile;
using Loon.ViewModels.Content.Write;
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
            services.AddSingleton<AppCommands>();

            // View Models
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<GetPinViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<HomeTimelineViewModel>();
            services.AddTransient<LikesTimelineViewModel>();
            services.AddTransient<UserProfileTimelineViewModel>();
            services.AddTransient<SearchTimelineViewModel>();
            services.AddTransient<WriteViewModel>();
            services.AddTransient<UserProfileViewModel>();

            // Models
            services.AddSingleton<ISettings, Settings>();

            // Services
            services.AddSingleton<ITwitterService, TwitterService>();
        }

        public static object GetService(Type type)
        {
            return ServiceProvider.GetService(type)
                ?? throw new NotSupportedException(type.Name);
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>()
                ?? throw new NotSupportedException(typeof(T).Name);
        }
    }
}