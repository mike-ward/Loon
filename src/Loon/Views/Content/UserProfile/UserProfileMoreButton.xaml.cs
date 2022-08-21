using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Loon.Views.Content.UserProfile
{
    public sealed class UserProfileMoreButton : UserControl
    {
        public UserProfileMoreButton()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenMenu()
        {
            var btn = this.FindDescendantOfType<Button>();
            btn?.ContextMenu?.Open(btn);
        }
    }
}