using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Write
{
    public class WriteView : UserControl
    {
        public WriteView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}