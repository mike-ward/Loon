using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemView : UserControl
    {
        public static readonly string TweetItemImageName = nameof(TweetItemImage);
        public static readonly string TweetItemProfileImageName = nameof(TweetItemProfileImage);
        public static readonly string TweetItemQuotedName = nameof(TweetItemQuoted);
        public static readonly string TweetItemRelatedName = nameof(TweetItemRelated);

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
            // Stop image downloading when scrolling
            this.FindControl<TweetItemImage>(TweetItemImageName).Clearing = true;
            this.FindControl<TweetItemProfileImage>(TweetItemProfileImageName).Clearing = true;
            this.FindControl<TweetItemQuoted>(TweetItemQuotedName).FindControl<TweetItemImage>(TweetItemImageName).Clearing = true;
            this.FindControl<TweetItemRelated>(TweetItemRelatedName).FindControl<TweetItemImage>(TweetItemImageName).Clearing = true;
        }
    }
}