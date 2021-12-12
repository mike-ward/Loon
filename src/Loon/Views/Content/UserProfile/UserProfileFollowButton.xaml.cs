using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileFollowButton : UserControl
    {
        public static readonly string TextBlockName = "tbn";

        public UserProfileFollowButton()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(System.EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (DataContext is User user &&
                this.FindControl<TextBlock>(TextBlockName) is TextBlock textBlock)
            {
                textBlock.Text = App.GetString(user.IsFollowing
                    ? "profile-following"
                    : "profile-follow");
            }
        }

        public void OnPointerEnter(object? sender, PointerEventArgs e)
        {
            if (sender is Border border &&
                DataContext is User user &&
                user.IsFollowing &&
                this.FindControl<TextBlock>(TextBlockName) is TextBlock textBlock)
            {
                border.Background = Application.Current.TryFindResource("RedHoverBrush", out var brush)
                    ? (IBrush)brush!
                    : Brushes.Black;
                textBlock.Text = App.GetString("profile-unfollow");
            }
        }

        public void OnPointerLeave(object? sender, PointerEventArgs e)
        {
            if (sender is Border border &&
                DataContext is User user &&
                this.FindControl<TextBlock>(TextBlockName) is TextBlock textBlock)
            {
                border.Background = Application.Current.TryFindResource("TwitterBlueBrush", out var brush)
                    ? (IBrush)brush!
                    : Brushes.Black;
                textBlock.Text = App.GetString(user.IsFollowing
                    ? "profile-following"
                    : "profile-follow");
            }
        }
    }
}