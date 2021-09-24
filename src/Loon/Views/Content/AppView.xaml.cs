using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    internal class AppView : UserControl
    {
        public AppView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}