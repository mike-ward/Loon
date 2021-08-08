using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Loon.Extensions;
using Loon.Models;
using Twitter.Models;

namespace Loon.Services
{
    internal static class FlowContentService
    {
        // Best I can do until Avalonia supports Inlines
        public static IEnumerable<Control> FlowContentInlines(TwitterStatus twitterStatus)
        {
            var nodes =
                twitterStatus.FlowContent as IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text)> ??
                FlowContentNodes(twitterStatus);

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

        public static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text)> FlowContentNodes(
            TwitterStatus twitterStatus)
        {
            var start         = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (item.Start >= start)
                {
                    var len  = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return (FlowContentNodeType.Text, text);
                }

                yield return (item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            yield return (FlowContentNodeType.Text, twitterString.Substring(start));
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>
            FlowControlItems(Entities entities)
        {
            var urls = entities.Urls
                    ?.Select(url =>
                    (
                        FlowContentNodeType: FlowContentNodeType.Url,
                        Text: url.ExpandedUrl,
                        Start: url.Indices[0],
                        End: url.Indices[1]
                    ))
                ?? Enumerable
                    .Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var mentions = entities.Mentions
                    ?.Select(mention =>
                    (
                        FlowContentNodeType: FlowContentNodeType.Mention,
                        Text: mention.ScreenName,
                        Start: mention.Indices[0],
                        End: mention.Indices[1]
                    ))
                ?? Enumerable
                    .Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var hashTags = entities.HashTags
                    ?.Select(hashtag =>
                    (
                        FlowContentNodeType: FlowContentNodeType.HashTag,
                        hashtag.Text,
                        Start: hashtag.Indices[0],
                        End: hashtag.Indices[1]
                    ))
                ?? Enumerable
                    .Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var media = entities.Media
                    ?.Select(mediaItem =>
                    (
                        FlowContentNodeType: FlowContentNodeType.Media,
                        Text: mediaItem.Url,
                        Start: mediaItem.Indices[0],
                        End: mediaItem.Indices[1]
                    ))
                ?? Enumerable
                    .Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

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
            textBlock.Text = text.HtmlDecode();
            return textBlock;
        }

        private static Control Hyperlink(string text, Action command, ContextMenu? contextMenu = null)
        {
            var button = new Button();
            button.Classes.Add("inline");
            button.Click       += delegate { command(); };
            button.ContextMenu =  contextMenu;

            var textBlock = new TextBlock();
            textBlock.Classes.Add("hyperlink");
            textBlock.Text = text.HtmlDecode();

            button.Content = textBlock;
            return button;
        }

        private static Control Link(string link)
        {
            return Hyperlink(
                link.TruncateWithEllipsis(25),
                () => OpenUrlService.Open(link),
                ContextMenu(link));
        }

        private static Control Mention(string screenName)
        {
            return Hyperlink(
                "@" + screenName,
                () => App.Commands.SetUserProfileContext.Execute(screenName));
        }

        private static Control Hashtag(string text)
        {
            var link = $"https://twitter.com/hashtag/{text}";
            return Hyperlink(
                "#" + text,
                () => OpenUrlService.Open(link),
                ContextMenu(link));
        }

        private static ContextMenu ContextMenu(string link)
        {
            var copyLinkAddress = new MenuItem {
                Header           = App.GetString("copy-link-address"),
                Command          = App.Commands.CopyToClipboard,
                CommandParameter = link
            };

            var emailLinkAddress = new MenuItem {
                Header           = App.GetString("email-link"),
                CommandParameter = link,
                Icon             = new TextBlock { Classes = new Classes("symbol", "padleft"), Text = App.GetString("MailSymbol") }
            };

            var shareLinkTwitter = new MenuItem {
                Header           = App.GetString("share-link-twitter"),
                CommandParameter = link,
                Icon = new TextBlock
                    { Classes = new Classes("symbol1", "padleft"), Text = App.GetString("TwitterSymbol") }
            };

            return new ContextMenu {
                HorizontalOffset = 15,
                VerticalOffset   = 10,
                Items = new[] {
                    copyLinkAddress,
                    emailLinkAddress,
                    shareLinkTwitter
                }
            };
        }
    }
}