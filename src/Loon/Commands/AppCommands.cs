using Loon.Interfaces;

namespace Loon.Commands
{
    public class AppCommands
    {
        public CloseAppCommand CloseApp { get; }
        public FollowAddRemoveCommand FollowAddRemove { get; }
        public GoToWriteTabCommand GoToWriteTab { get; }
        public LikesAddRemoveCommand LikesAddRemove { get; }
        public MinimizeAppCommand MinimizeApp { get; }
        public ReplyToCommand ReplyTo { get; }
        public RetweetCommand Retweet { get; }
        public SetUserProfileContextCommand SetUserProfileContext { get; }
        public SignoutCommand Signout { get; }
        public TabGoBackCommand TabGoBack { get; }

        public AppCommands(ITwitterService twitterService, ISettings settings)
        {
            CloseApp = new CloseAppCommand();
            FollowAddRemove = new FollowAddRemoveCommand(twitterService);
            GoToWriteTab = new GoToWriteTabCommand();
            LikesAddRemove = new LikesAddRemoveCommand(twitterService);
            MinimizeApp = new MinimizeAppCommand();
            ReplyTo = new ReplyToCommand(twitterService);
            Retweet = new RetweetCommand(settings, twitterService);
            SetUserProfileContext = new SetUserProfileContextCommand(twitterService);
            Signout = new SignoutCommand(settings);
            TabGoBack = new TabGoBackCommand();
        }
    }
}