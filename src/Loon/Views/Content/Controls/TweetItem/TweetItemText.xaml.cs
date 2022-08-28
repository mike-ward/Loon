using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
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

            var inlines    = FlowContentService.FlowContentInlines(status, token);
            var blocks     = ToCollections(inlines);
            var stackPanel = (StackPanel)Content!;
            stackPanel.Children.Clear();

            foreach (var block in blocks)
            {
                stackPanel.Children.Add(new RichTextBlock
                {
                    Inlines      = block,
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }
        }

        private static IEnumerable<InlineCollection> ToCollections(IEnumerable<Inline> inlines)
        {
            var inlineCollections = new List<InlineCollection>();
            var inlineCollection  = new InlineCollection();

            foreach (var inline in inlines)
            {
                if (inline is LineBreak && inlineCollection.Count > 0)
                {
                    inlineCollections.Add(inlineCollection);
                    inlineCollection = new InlineCollection();
                }

                if (inline is LineBreak) continue;
                inlineCollection.Add(inline);
            }

            if (inlineCollection.Count > 0) inlineCollections.Add(inlineCollection);
            return inlineCollections;
        }
    }
}