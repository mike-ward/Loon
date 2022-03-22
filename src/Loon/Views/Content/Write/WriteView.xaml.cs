using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Write;

namespace Loon.Views.Content.Write
{
    public sealed class WriteView : UserControl
    {
        public WriteView()
        {
            DataContext = App.ServiceProvider.GetService<WriteViewModel>();
            AvaloniaXamlLoader.Load(this);
        }
    }
}