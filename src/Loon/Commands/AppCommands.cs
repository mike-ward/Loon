using Loon.Interfaces;

namespace Loon.Commands
{
    public class AppCommands
    {
        public FollowAddRemoveCommand FollowAddRemove { get; }
        public GoToWriteTabCommand GoToWriteTab { get; }
        public LikesAddRemoveCommand LikesAddRemove { get; }
        public ReplyToCommand ReplyTo { get; }
        public RetweetCommand Retweet { get; }
        public SignoutCommand Signout { get; }
        public TabGoBackCommand TabGoBack { get; }

        public AppCommands(ITwitterService twitterService, ISettings settings)
        {
            FollowAddRemove = new FollowAddRemoveCommand(twitterService);
            GoToWriteTab = new GoToWriteTabCommand();
            LikesAddRemove = new LikesAddRemoveCommand(twitterService);
            ReplyTo = new ReplyToCommand(twitterService);
            Retweet = new RetweetCommand(settings, twitterService);
            Signout = new SignoutCommand(settings);
            TabGoBack = new TabGoBackCommand();
        }
    }
}