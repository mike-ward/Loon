﻿using Loon.Interfaces;

namespace Loon.Commands
{
    public sealed class AppCommands
    {
        public AddToHiddenImagesCommand     AddToHiddenImages     { get; }
        public ClearHiddenImageCacheCommand ClearHiddenImageCache { get; }
        public ClearImageCacheCommand       ClearImageCache       { get; }
        public CloseAppCommand              CloseApp              { get; }
        public CopyToClipboardCommand       CopyToClipboard       { get; }
        public FollowAddRemoveCommand       FollowAddRemove       { get; }
        public OpenWriteTabCommand          OpenWriteTab          { get; }
        public LikesAddRemoveCommand        LikesAddRemove        { get; }
        public MinimizeAppCommand           MinimizeApp           { get; }
        public OpenUrlCommand               OpenUrl               { get; }
        public RetweetCommand               Retweet               { get; }
        public SetUserProfileContextCommand SetUserProfileContext { get; }
        public SignoutCommand               Signout               { get; }
        public ShowAppCommand               ShowApp               { get; }

        public AppCommands(ITwitterService twitterService, ISettings settings)
        {
            AddToHiddenImages     = new AddToHiddenImagesCommand(settings);
            ClearHiddenImageCache = new ClearHiddenImageCacheCommand(settings);
            ClearImageCache       = new ClearImageCacheCommand();
            CloseApp              = new CloseAppCommand();
            CopyToClipboard       = new CopyToClipboardCommand();
            FollowAddRemove       = new FollowAddRemoveCommand(twitterService);
            OpenWriteTab          = new OpenWriteTabCommand();
            LikesAddRemove        = new LikesAddRemoveCommand(twitterService);
            MinimizeApp           = new MinimizeAppCommand();
            OpenUrl               = new OpenUrlCommand();
            Retweet               = new RetweetCommand(settings, twitterService);
            SetUserProfileContext = new SetUserProfileContextCommand();
            Signout               = new SignoutCommand(settings);
            ShowApp               = new ShowAppCommand();
        }
    }
}