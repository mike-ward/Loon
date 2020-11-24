using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemText : UserControl
    {
        public TweetItemText()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
