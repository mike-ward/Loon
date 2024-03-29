﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Twitter.Services;

namespace Twitter.Models
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TwitterStatus : INotifyPropertyChanged
    {
        private int              replyCount;
        private int              retweetCount;
        private int              favoriteCount;
        private int              quoteCount;
        private DateTime         createdDate;
        private RelatedLinkInfo? relatedLinkInfo;
        private bool             retweetedByMe;
        private bool             favorited;
        private bool             isSensitive;
        private string?          translatedText;
        private string?          language;

        [JsonPropertyName("id_str")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("full_text")]
        public string? FullText { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; } = User.Empty;

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonPropertyName("entities")]
        public Entities? Entities { get; set; }

        [JsonPropertyName("extended_entities")]
        public Entities? ExtendedEntities { get; set; }

        public bool IsQuoted => QuotedStatus is not null;

        [JsonPropertyName("quoted_status")]
        public TwitterStatus? QuotedStatus { get; set; }

        [JsonPropertyName("quote_count")]
        public int QuoteCount
        {
            get => quoteCount;
            set => SetProperty(ref quoteCount, value);
        }

        public bool IsRetweet => RetweetedStatus is not null;

        [JsonPropertyName("retweeted_status")]
        public TwitterStatus? RetweetedStatus { get; set; }

        [JsonPropertyName("retweeted")]
        public bool RetweetedByMe
        {
            get => retweetedByMe;
            set => SetProperty(ref retweetedByMe, value);
        }

        [JsonPropertyName("retweet_count")]
        public int RetweetCount
        {
            get => retweetCount;
            set => SetProperty(ref retweetCount, value);
        }

        [JsonPropertyName("favorite_count")]
        public int FavoriteCount
        {
            get => favoriteCount;
            set => SetProperty(ref favoriteCount, value);
        }

        [JsonPropertyName("favorited")]
        public bool Favorited
        {
            get => favorited;
            set => SetProperty(ref favorited, value);
        }

        [JsonPropertyName("in_reply_to_status_id_str")]
        public string? InReplyToStatusId { get; set; }

        [JsonPropertyName("in_reply_to_user_id_str")]
        public string? InReplyToUserId { get; set; }

        [JsonPropertyName("in_reply_to_screen_name")]
        public string? InReplyToScreenName { get; set; }

        [JsonPropertyName("reply_count")]
        public int ReplyCount
        {
            get => replyCount;
            set => SetProperty(ref replyCount, value);
        }

        [JsonPropertyName("possibly_sensitive")]
        public bool IsSensitive
        {
            get => isSensitive;
            set => SetProperty(ref isSensitive, value);
        }

        [JsonPropertyName("lang")]
        public string? Language
        {
            get => language;
            set => SetProperty(ref language, value);
        }

        [JsonIgnore]
        public string? TranslatedText
        {
            get => translatedText;
            set => SetProperty(ref translatedText, value);
        }

        [JsonIgnore]
        protected string? OverrideLink { get; init; }

        [JsonIgnore]
        public RelatedLinkInfo? RelatedLinkInfo
        {
            get => relatedLinkInfo;
            set => SetProperty(ref relatedLinkInfo, value);
        }

        /// <summary>
        ///     Originating status is what get's displayed
        /// </summary>
        [JsonIgnore]
        public TwitterStatus OriginatingStatus => IsRetweet
            ? RetweetedStatus ?? throw new InvalidOperationException("Invalid program state")
            : this;

        /// <summary>
        ///     Create a link to a twitter status
        /// </summary>
        [JsonIgnore]
        public string StatusLink => string.IsNullOrWhiteSpace(OverrideLink)
            ? $"https://twitter.com/{User.ScreenName}/status/{Id}"
            : OverrideLink;

        /// <summary>
        ///     Converts a serialized twitter date into a System.DateTime object and caches it.
        /// </summary>
        [JsonIgnore]
        public DateTime CreatedDate
        {
            get
            {
                if (createdDate == default)
                {
                    createdDate = ParseTwitterDate(CreatedAt);
                }

                return createdDate;
            }
        }

        [JsonIgnore]
        public object? FlowContent { get; set; }

        /// <summary>
        ///     Indicates if user is author of tweet
        /// </summary>
        [JsonIgnore]
        public bool IsMyTweet { get; set; }

        public bool MentionsMe { get; set; }

        public const string TwitterDateTimeFormat = "ddd MMM dd HH:mm:ss zzz yyyy";

        public static DateTime ParseTwitterDate(string? s)
        {
            return string.IsNullOrWhiteSpace(s)
                ? default
                : DateTime.ParseExact(
                    s,
                    TwitterDateTimeFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal);
        }

        /// <summary>
        ///     Update a status's counts from a newer status
        /// </summary>
        /// <param name="status"></param>
        public void UpdateFromStatus(TwitterStatus? status)
        {
            if (status is null) return;
            if (Id is null) throw new InvalidOperationException("Status Id is null");
            if (!Id.Equals(status.Id, StringComparison.Ordinal)) return;

            ReplyCount    = status.ReplyCount;
            RetweetCount  = status.RetweetCount;
            FavoriteCount = status.FavoriteCount;
            QuoteCount    = status.QuoteCount;
            RetweetedByMe = status.RetweetedByMe;
            Favorited     = status.Favorited;

            var userConnections = UserConnectionsService.LookupUserConnections(User.Id);
            User.IsFollowing  = userConnections?.IsFollowing ?? false;
            User.IsFollowedBy = userConnections?.IsFollowedBy ?? false;

            QuotedStatus?.UpdateFromStatus(status.QuotedStatus);
            RetweetedStatus?.UpdateFromStatus(status.RetweetedStatus);
        }

        public void UpdateAboutMeProperties(string? screenName)
        {
            if (string.IsNullOrEmpty(screenName)) return;
            IsMyTweet  = string.CompareOrdinal(screenName, OriginatingStatus.User.ScreenName) == 0;
            MentionsMe = Entities?.Mentions?.Any(mention => string.CompareOrdinal(mention.ScreenName, screenName) == 0) ?? false;
        }

        /// <summary>
        ///     Tricks the UI into updating the time ago dates in the timeline
        /// </summary>
        public void InvokeUpdateTimeStamp()
        {
            PropertyChanged?.Invoke(OriginatingStatus, eventArgsCache.GetOrAdd(nameof(CreatedDate), name => new PropertyChangedEventArgs(name)));
        }

        // Overrides
        //
        public override bool Equals(object? obj)
        {
            return obj is TwitterStatus twitterStatus && Id.Equals(twitterStatus.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return StringComparer.Ordinal.GetHashCode(Id);
        }

        // INotifyPropertyChanged Implementation
        //
        public event PropertyChangedEventHandler? PropertyChanged;

        private void SetProperty<T>([NotNullIfNotNull("value")] ref T item, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return;
            item = value;
            OnPropertyChanged(propertyName);
        }

        private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> eventArgsCache = new(StringComparer.Ordinal);

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, eventArgsCache.GetOrAdd(propertyName!, name => new PropertyChangedEventArgs(name)));
        }
    }
}