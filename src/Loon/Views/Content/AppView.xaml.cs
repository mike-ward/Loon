using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content
{
    internal class AppView : UserControl
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