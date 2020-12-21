using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileName : UserControl
    {
        public UserProfileName()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}