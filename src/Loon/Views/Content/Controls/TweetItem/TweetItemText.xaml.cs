using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
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

            var inlines          = FlowContentService.FlowContentInlines(status, token);
            var filteredInlines  = RemoveConsecutiveLineFeeds(inlines);
            var inlineCollection = new InlineCollection();
            inlineCollection.AddRange(filteredInlines);

            var richTextBlock = (RichTextBlock)Content!;
            richTextBlock.Inlines = inlineCollection;
        }

        private static IEnumerable<Inline> RemoveConsecutiveLineFeeds(IEnumerable<Inline> lines)
        {
            Inline previous = new Run();

            foreach (var line in lines)
            {
                if (line is LineBreak && previous is LineBreak) continue;
                previous = line;
                yield return line;
            }
        }
    }
}