using Loon.Interfaces;

namespace Loon.Commands
{
    public class AppCommands
    {
        public CloseAppCommand CloseApp { get; }
        public FollowAddRemoveCommand FollowAddRemove { get; }
        public OpenWriteTabCommand OpenWriteTab { get; }
        public LikesAddRemoveCommand LikesAddRemove { get; }
        public MinimizeAppCommand MinimizeApp { get; }
        public RetweetCommand Retweet { get; }
        public SetUserProfileContextCommand SetUserProfileContext { get; }
        public SignoutCommand Signout { get; }

        public AppCommands(ITwitterService twitterService, ISettings settings, IPubSubService pubSubService)
        {
            CloseApp = new CloseAppCommand();
            FollowAddRemove = new FollowAddRemoveCommand(twitterService);
            OpenWriteTab = new OpenWriteTabCommand(pubSubService);
            LikesAddRemove = new LikesAddRemoveCommand(twitterService);
            MinimizeApp = new MinimizeAppCommand();
            Retweet = new RetweetCommand(settings, twitterService);
            SetUserProfileContext = new SetUserProfileContextCommand(twitterService, pubSubService);
            Signout = new SignoutCommand(settings);
        }
    }
}