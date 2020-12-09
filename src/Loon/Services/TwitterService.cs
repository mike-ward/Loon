using System.Collections.Generic;
using System.Threading.Tasks;
using Loon.Interfaces;
using Twitter.Models;
using Twitter.Services;

namespace Loon.Services
{
    internal class TwitterService : ITwitterService
    {
        private readonly TwitterApi twitterApi;

        public TwitterService()
        {
            const string consumerKey = "ZScn2AEIQrfC48Zlw";
            const string consumerSecret = "8gKdPBwUfZCQfUiyeFeEwVBQiV3q50wIOrIjoCxa2Q";
            twitterApi = new TwitterApi(consumerKey, consumerSecret);
        }

        public ValueTask<OAuthTokens> AuthenticateWithPinAsync(OAuthTokens requestTokens, string pin) => twitterApi.AuthenticateWithPin(requestTokens, pin);

        public void AuthenticationTokens(string? accessToken, string? accessTokenSecret) => twitterApi.AuthenticationTokens(accessToken, accessTokenSecret);

        public ValueTask<OAuthTokens> GetPin() => twitterApi.GetPinAsync();

        public ValueTask<IEnumerable<TwitterStatus>> GetHomeTimeline() => twitterApi.HomeTimeline();

        public ValueTask<IEnumerable<TwitterStatus>> GetMentionsTimeline(int count = 20) => twitterApi.MentionsTimeline(count);

        public ValueTask<IEnumerable<TwitterStatus>> GetFavoritesTimeline() => twitterApi.FavoritesTimeline();

        public ValueTask<User> UserInfo(string screenName) => twitterApi.UserInfo(screenName);

        public ValueTask<Tweet> Search(string query) => twitterApi.Search(query);

        public ValueTask RetweetStatus(string statusId) => twitterApi.RetweetStatus(statusId);

        public ValueTask UnretweetStatus(string statusId) => twitterApi.UnretweetStatus(statusId);

        public ValueTask CreateFavorite(string statusId) => twitterApi.CreateFavorite(statusId);

        public ValueTask DestroyFavorite(string statusId) => twitterApi.DestroyFavorite(statusId);

        public ValueTask Follow(string screenName) => twitterApi.Follow(screenName);

        public ValueTask Unfollow(string screenName) => twitterApi.Unfollow(screenName);

        public ValueTask<TwitterStatus> UpdateStatus(string text, string? replyToStatusId, string? attachmentUrl, string[]? mediaIds)
            => twitterApi.UpdateStatus(text, replyToStatusId, attachmentUrl, mediaIds);

        public ValueTask<TwitterStatus> GetStatus(string statusId) => twitterApi.GetStatus(statusId);

        public ValueTask<UploadMedia> UploadMediaInit(int totalBytes, string mediaType) => twitterApi.UploadMediaInit(totalBytes, mediaType);

        public ValueTask UploadMediaAppend(string mediaId, int segmentIndex, byte[] data) => twitterApi.UploadMediaAppend(mediaId, segmentIndex, data);

        public ValueTask<UploadMedia> UploadMediaStatus(string mediaId) => twitterApi.UploadMediaStatus(mediaId);

        public ValueTask<UploadMedia> UploadMediaFinalize(string mediaId) => twitterApi.UploadMediaFinalize(mediaId);

        public ValueTask<IEnumerable<UserConnection>> GetFriendships(string[] ids) => twitterApi.GetFriendships(ids);
    }
}