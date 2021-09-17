using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Views.Content.Controls.TweetItem;

namespace Loon.Views.Content.Timelines
{
    [SuppressMessage("ReSharper", "ConvertToConstant.Global")]
    internal class TimelineView : UserControl
    {
        public static readonly string ScrollViewerName  = "ScrollViewer";
        public static readonly string ItemsRepeaterName = "ItemsRepeater";

        public TimelineView()
        {
            InitializeComponent();

            var itemsRepeater = this.FindControl<ItemsRepeater>(ItemsRepeaterName);
            if (itemsRepeater is not null)
            {
                itemsRepeater.ElementPrepared += (_, args) => ((TweetItemView)args.Element).CancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(7));
                itemsRepeater.ElementClearing += (_, args) =>
                {
                    var tweetItemView = (TweetItemView)args.Element;
                    tweetItemView.CancellationTokenSource.Cancel();
                    tweetItemView.CancellationTokenSource = new CancellationTokenSource();
                };
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == MouseButton.Right)
            {
                e.Handled = true;
                this.FindControl<ScrollViewer>(ScrollViewerName)?.ScrollToHome();
            }

            base.OnPointerReleased(e);
        }
    }
}