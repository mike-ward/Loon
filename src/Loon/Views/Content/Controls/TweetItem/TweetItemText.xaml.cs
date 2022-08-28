using System;
using System.Collections.Generic;
using System.Linq;
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
            var paragraphs = ToParagraphs(inlines);
            var stackPanel = (StackPanel)Content!;
            stackPanel.Children.Clear();

            foreach (var paragraph in paragraphs)
            {
                stackPanel.Children.Add(new RichTextBlock
                {
                    Inlines      = paragraph,
                    TextWrapping = TextWrapping.WrapWithOverflow
                });
            }
        }

        private static IEnumerable<InlineCollection> ToParagraphs(IEnumerable<Inline> inlinesArg)
        {
            var inlines = inlinesArg.ToArray();
            var start   = 0;
            var end     = inlines.Length;

            for (var i = 0; i < end; ++i)
            {
                if (inlines[i] is LineBreak)
                {
                    // paragraph is two or more consecutive line breaks
                    var lineBreaks = 1;
                    while (++i < end && inlines[i] is LineBreak)
                    {
                        lineBreaks += 1;
                    }

                    if (lineBreaks > 1)
                    {
                        var inlineCollection = new InlineCollection();
                        inlineCollection.AddRange(inlines[start..(i - lineBreaks)]);
                        yield return inlineCollection;
                        start = i;
                    }
                }
            }

            if (start < end)
            {
                var inlineCollection = new InlineCollection();
                inlineCollection.AddRange(inlines[start..end]);
                yield return inlineCollection;
            }
        }
    }
}