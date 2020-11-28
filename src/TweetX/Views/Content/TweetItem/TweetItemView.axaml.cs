using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TweetX.Views.Content.TweetItem
{
    public class TweetItemView : UserControl
    {
        public TweetItemView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Clearing()
        {
            this.FindControl<TweetItemImage>(nameof(TweetItemImage)).Clearing = true;
            this.FindControl<TweetItemProfileImage>(nameof(TweetItemProfileImage)).Clearing = true;
            this.FindControl<TweetItemRelated>(nameof(TweetItemRelated)).FindControl<TweetItemImage>(nameof(TweetItemImage)).Clearing = true;
        }
    }
}