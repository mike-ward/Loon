using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Services;
using Loon.ViewModels.Content.UserProfile;

namespace Loon.Views.Content.UserProfile
{
    internal class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
            DataContext = Bootstrapper.ServiceProvider.GetService<UserProfileViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}