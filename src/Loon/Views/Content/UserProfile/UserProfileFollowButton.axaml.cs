using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using Twitter.Models;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileFollowButton : UserControl
    {
        public UserProfileFollowButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(System.EventArgs e)
        {
            base.OnDataContextChanged(e);
            if (DataContext is User user && this.FindDescendantOfType<TextBlock>() is TextBlock textBlock)
            {
                textBlock.Text = App.GetString(user.IsFollowing ? "following" : "follow");
            }
        }

        public void OnPointerEnter(object? sender, PointerEventArgs e)
        {
            if (sender is Border border && DataContext is User user && user.IsFollowing)
            {
                border.Background = App.Current.TryFindResource("RedHoverBrush", out var brush) ? (ISolidColorBrush)brush! : Brushes.Black;
                border.FindDescendantOfType<TextBlock>().Text = App.GetString("unfollow");
            }
        }

        public void OnPointerLeave(object? sender, PointerEventArgs e)
        {
            if (sender is Border border && DataContext is User user)
            {
                border.Background = App.Current.TryFindResource("TwitterBlueBrush", out var brush) ? (ISolidColorBrush)brush! : Brushes.Black;
                border.FindDescendantOfType<TextBlock>().Text = App.GetString(user.IsFollowing ? "following" : "follow");
            }
        }
    }
}