using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.ViewModels.Content.UserProfile;

namespace Loon.Views.Content.UserProfile
{
    internal class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<UserProfileViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}