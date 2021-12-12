using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    public class AppView : UserControl
    {
        public AppView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}