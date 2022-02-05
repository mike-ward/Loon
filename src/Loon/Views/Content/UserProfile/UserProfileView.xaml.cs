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
            DataContext = App.ServiceProvider.GetService<UserProfileViewModel>();
            AvaloniaXamlLoader.Load(this);
        }
    }
}