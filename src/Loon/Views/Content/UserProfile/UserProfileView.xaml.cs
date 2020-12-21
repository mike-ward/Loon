using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}