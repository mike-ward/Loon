using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    public class AppView : UserControl
    {
        public AppView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}