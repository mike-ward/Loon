using System;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemText : UserControl
    {
        public TweetItemText()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if (DataContext is TwitterStatus status &&
                this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>() is { } cancellationTokeSourceProvider)
            {
                var token = cancellationTokeSourceProvider.CancellationTokenSource.Token;
                if (token.IsCancellationRequested) return;
                var wrapPanel = this.FindControl<WrapPanel>("Container");
                wrapPanel.Children.AddRange(FlowContentService.FlowContentInlines(status, token));
            }
        }
    }
}