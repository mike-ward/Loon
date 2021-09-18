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
    internal class TweetItemText : UserControl
    {
        private CancellationToken cancellationToken = CancellationToken.None;

        public TweetItemText()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            if (this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>() is { } cancellationTokeSourceProvider)
            {
                cancellationToken = cancellationTokeSourceProvider.CancellationTokenSource.Token;
            }

            base.OnDataContextChanged(e);
        }

        private void ItemsControl_OnDataContextChanged(object? sender, EventArgs e)
        {
            var token = cancellationToken;
            if (token.IsCancellationRequested) return;
            
            var itemsControl = this.FindControl<ItemsControl>("ItemsControl");
            if (token.IsCancellationRequested) return;
            
            if (DataContext is TwitterStatus status)
            {
                itemsControl.Items = FlowContentService.FlowContentInlines(status, token);
            }
        }
    }
}