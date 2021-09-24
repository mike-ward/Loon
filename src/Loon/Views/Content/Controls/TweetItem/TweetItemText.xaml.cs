using System;
using System.Threading.Tasks;
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

                var itemsControl = this.FindControl<ItemsControl>("ItemsControl");
                if (token.IsCancellationRequested) return;

                try
                {
                    itemsControl.Items = FlowContentService.FlowContentInlines(status, token);
                }
                catch (TaskCanceledException)
                {
                    // eat it.
                }
            }
        }
    }
}