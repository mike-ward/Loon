using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content.TweetBlock
{
    public class TweetBlockView : UserControl
    {
        public TweetBlockView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}