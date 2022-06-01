using System;
using Twitter.Models;

namespace Loon.Models
{
    public class DonateNagStatus : TwitterStatus
    {
        public const  string DonateNagStatusId = "1";
        private const string donateUrl         = "https://mike-ward.net/donate";

        public DonateNagStatus()
        {
            Id           = DonateNagStatusId;
            FullText     = "Please consider donating to Loon.\nhttps://mike-ward.net/donate";
            CreatedAt    = DateTime.UtcNow.ToString(TwitterStatus.TwitterDateTimeFormat);
            OverrideLink = donateUrl;
            Language     = "en";

            Entities = new Entities
            {
                Urls = new[]
                {
                    new UrlEntity
                    {
                        DisplayUrl  = donateUrl,
                        ExpandedUrl = donateUrl,
                        Indices     = new[] { 36, 64 }
                    }
                }
            };

            ExtendedEntities = new Entities
            {
                Media = new[]
                {
                    new Media
                    {
                        MediaUrl = "https://mike-ward.net/cdn/images/donate.png"
                    }
                }
            };

            User = new User
            {
                ScreenName      = "mikeward_aa",
                Name            = "Mike Ward",
                Id              = "14410002",
                Location        = "Dubuque, IA",
                ProfileImageUrl = "https://pbs.twimg.com/profile_images/495209879749935104/AV0xDcXP_normal.jpeg",
                Description     = ".NET, Technology, Life, Whatever..."
            };
        }
    }
}