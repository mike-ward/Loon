using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.ViewModels.Content.Write;

namespace Loon.Views.Content.Write
{
    internal class WriteView : UserControl
    {
        public WriteView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<WriteViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}