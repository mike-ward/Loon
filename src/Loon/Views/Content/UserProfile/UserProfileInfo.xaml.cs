using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileInfo : UserControl
    {
        public UserProfileInfo()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}