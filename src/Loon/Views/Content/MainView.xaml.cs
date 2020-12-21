using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}