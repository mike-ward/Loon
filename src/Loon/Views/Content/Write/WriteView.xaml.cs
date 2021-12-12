using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Write;

namespace Loon.Views.Content.Write
{
    public class WriteView : UserControl
    {
        public WriteView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<WriteViewModel>();
        }
    }
}