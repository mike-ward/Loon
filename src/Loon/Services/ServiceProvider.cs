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
    public class ServiceProvider
    {
        private readonly Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider;

        public ServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(AppCommands));
            services.AddTransient(typeof(MainWindowViewModel));
            services.AddTransient(typeof(GetPinViewModel));
            services.AddTransient(typeof(HomeTimelineViewModel));
            services.AddTransient(typeof(LikesTimelineViewModel));
            services.AddTransient(typeof(UserProfileTimelineViewModel));
            services.AddTransient(typeof(SearchTimelineViewModel));
            services.AddTransient(typeof(WriteViewModel));
            services.AddTransient(typeof(UserProfileViewModel));
            services.AddSingleton(typeof(ISettings), typeof(Settings));
            services.AddSingleton(typeof(ITwitterService), typeof(TwitterService));
        }

        public object GetService(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return serviceProvider.GetService(type) ?? throw new NotSupportedException(type.Name);
        }

        public T GetService<T>()
        {
            return serviceProvider.GetService<T>() ?? throw new NotSupportedException(typeof(T).Name);
        }
    }
}