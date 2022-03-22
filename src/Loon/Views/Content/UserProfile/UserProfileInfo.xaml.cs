using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.UserProfile
{
    public sealed class UserProfileInfo : UserControl
    {
        public UserProfileInfo()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}