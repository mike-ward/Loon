using Loon.Interfaces;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace Loon.Commands
{
    public sealed class AppCommands
    {
        private readonly ITwitterService twitterService;
        private readonly ISettings       settings;

        // Return new objects to keep callers from getting rooted.
        
        public AddToHiddenImagesCommand     AddToHiddenImages     => new(settings);
        public ClearHiddenImageCacheCommand ClearHiddenImageCache => new(settings);
        public ClearImageCacheCommand       ClearImageCache       => new();
        public CloseAppCommand              CloseApp              => new();
        public CopyToClipboardCommand       CopyToClipboard       => new();
        public FollowAddRemoveCommand       FollowAddRemove       => new(twitterService);
        public OpenWriteTabCommand          OpenWriteTab          => new();
        public LikesAddRemoveCommand        LikesAddRemove        => new(twitterService);
        public MinimizeAppCommand           MinimizeApp           => new();
        public OpenUrlCommand               OpenUrl               => new();
        public RetweetCommand               Retweet               => new(settings, twitterService);
        public SetUserProfileContextCommand SetUserProfileContext => new();
        public SignoutCommand               Signout               => new(settings);
        public ShowAppCommand               ShowApp               => new();

        public AppCommands(ITwitterService twitterService, ISettings settings)
        {
            this.twitterService = twitterService;
            this.settings       = settings;
        }
    }
}