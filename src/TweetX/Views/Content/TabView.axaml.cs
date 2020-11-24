using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content
{
    public class TabView : UserControl
    {
        public TabView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}