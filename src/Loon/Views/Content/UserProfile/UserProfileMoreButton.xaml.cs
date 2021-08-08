using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Loon.Views.Content.UserProfile
{
    internal class UserProfileMoreButton : UserControl
    {
        public UserProfileMoreButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenMenu()
        {
            var btn = this.FindDescendantOfType<Button>();
            btn.ContextMenu?.Open(btn);
        }
    }
}