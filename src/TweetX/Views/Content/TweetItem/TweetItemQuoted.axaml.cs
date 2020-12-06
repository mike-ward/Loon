using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemQuoted : UserControl
    {
        public TweetItemQuoted()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}