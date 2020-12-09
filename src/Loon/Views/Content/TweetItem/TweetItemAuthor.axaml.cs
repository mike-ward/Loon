using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.TweetItem
{
    public class TweetItemAuthor : UserControl
    {
        public TweetItemAuthor()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}