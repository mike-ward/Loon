using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    public sealed class AppView : UserControl
    {
        public AppView()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}