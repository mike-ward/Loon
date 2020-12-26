using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Avalonia.Controls;
using Loon.Extensions;
using Loon.Models;
using Loon.ViewModels;
using Twitter.Models;

namespace Loon.Services
{
    internal static class FlowContentService
    {
        // Best I can do until Avalonia supports Inlines
        public static IEnumerable<Control> FlowContentInlines(TwitterStatus twitterStatus)
        {
            if (twitterStatus is null)
            {
                yield break;
            }

            var nodes = FlowContentNodes(twitterStatus);

            foreach (var node in nodes)
            {
                switch (node.FlowContentNodeType)
                {
                    case FlowContentNodeType.Text:
                        yield return Run(node.Text);
                        break;

                    case FlowContentNodeType.Url:
                        yield return Link(node.Text);
                        break;

                    case FlowContentNodeType.Mention:
                        yield return Mention(node.Text);
                        break;

                    case FlowContentNodeType.HashTag:
                        yield return Hashtag(node.Text);
                        break;

                    case FlowContentNodeType.Media:
                        // Media is handled else where
                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text)> FlowContentNodes(TwitterStatus twitterStatus)
        {
            var start = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (item.Start >= start)
                {
                    var len = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return (FlowContentNodeType.Text, text);
                }

                yield return (item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            yield return (FlowContentNodeType.Text, twitterString.Substring(start));
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)> FlowControlItems(Entities entities)
        {
            var urls = entities.Urls
                 ?.Select(url =>
                 (
                     FlowContentNodeType: FlowContentNodeType.Url,
                     Text: url.ExpandedUrl,
                     Start: url.Indices[0],
                     End: url.Indices[1]
                 ))
                 ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var mentions = entities.Mentions
                ?.Select(mention =>
                (
                    FlowContentNodeType: FlowContentNodeType.Mention,
                    Text: mention.ScreenName,
                    Start: mention.Indices[0],
                    End: mention.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var hashTags = entities.HashTags
                ?.Select(hashtag =>
                (
                    FlowContentNodeType: FlowContentNodeType.HashTag,
                    Text: hashtag.Text,
                    Start: hashtag.Indices[0],
                    End: hashtag.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var media = entities.Media
                ?.Select(media =>
                (
                    FlowContentNodeType: FlowContentNodeType.Media,
                    Text: media.Url,
                    Start: media.Indices[0],
                    End: media.Indices[1]
                ))
                ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            return urls
                .Concat(mentions)
                .Concat(hashTags)
                .Concat(media)
                .OrderBy(o => o.Start);
        }

        private static TextBlock Run(string text)
        {
            var textBlock = new TextBlock();
            textBlock.Classes.Add("normal");
            textBlock.Text = ConvertXmlEntities(text);
            return textBlock;
        }

        private static Control Hyperlink(string text, Action command)
        {
            var button = new Button();
            button.Classes.Add("inline");
            button.Click += delegate { command(); };

            var textBlock = new TextBlock();
            textBlock.Classes.Add("hyperlink");
            textBlock.Text = ConvertXmlEntities(text);

            button.Content = textBlock;
            return button;
        }

        private static Control Link(string link)
        {
            return Hyperlink(
                link.TruncateWithEllipsis(25),
                () => OpenUrlService.Open(link));
        }

        private static Control Mention(string screenName)
        {
            return Hyperlink(
                "@" + screenName,
                 () => (App.MainWindow.DataContext as MainWindowViewModel)?.SetUser(screenName));
        }

        private static Control Hashtag(string text)
        {
            return Hyperlink(
                "#" + text,
                () => OpenUrlService.Open($"https://twitter.com/hashtag/{text}"));
        }

        private static string ConvertXmlEntities(string text)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return WebUtility.HtmlDecode(
                WebUtility.HtmlDecode(text));
        }
    }
}