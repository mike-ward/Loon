using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.UserProfile
{
    public class UserProfileBackBar : UserControl
    {
        public UserProfileBackBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}