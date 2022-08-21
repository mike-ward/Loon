using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public sealed class TweetItemText : UserControl
    {
        public TweetItemText()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if (DataContext is not TwitterStatus status) return;

            var token = this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>()?.CancellationTokenSource.Token ?? CancellationToken.None;
            if (token.IsCancellationRequested) return;

            var panel = this.FindControl<Panel>("Container");
            panel?.Children.Clear();
            panel?.Children.AddRange(FlowContentService.FlowContentInlines(status, token));
        }
    }
}