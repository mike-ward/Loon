using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemQuoted : UserControl
    {
        public TweetItemQuoted()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}