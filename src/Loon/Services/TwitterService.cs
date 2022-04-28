using Loon.Interfaces;
using Twitter.Services;

namespace Loon.Services
{
    internal sealed class TwitterService : ITwitterService
    {
        public TwitterApi TwitterApi { get; }

        public TwitterService()
        {
            const string consumerKey    = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";
            TwitterApi = new TwitterApi(consumerKey, consumerSecret);
        }
    }
}