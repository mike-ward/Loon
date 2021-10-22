using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Jab;
using Loon.Converters;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Twitter.Models;

namespace Loon.Services
{
    internal static class FlowContentService
    {
        // Best I can do until Avalonia supports Inlines
        public static IEnumerable<Control> FlowContentInlines(TwitterStatus twitterStatus, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) yield break;

            if (twitterStatus.FlowContent is null)
            {
                var content = FlowContentNodes(twitterStatus, cancellationToken).ToArray();
                if (cancellationToken.IsCancellationRequested) yield break;
                twitterStatus.FlowContent = content;
            }

            foreach (var (nodeType, text) in ((FlowContentNodeType, string )[])twitterStatus.FlowContent)
            {
                if (cancellationToken.IsCancellationRequested) yield break;

                var control = nodeType switch {
                    FlowContentNodeType.Text    => Run(text),
                    FlowContentNodeType.Url     => Url(text),
                    FlowContentNodeType.Mention => Mention(text),
                    FlowContentNodeType.HashTag => Hashtag(text),
                    FlowContentNodeType.Media   => null, // images handled elsewhere
                    _                           => throw new ConstraintException("invalid FlowContentNodeType")
                };

                if (control is null) continue;
                yield return control;
            }
        }

        private static IEnumerable<(FlowContentNodeType, string)> FlowContentNodes(TwitterStatus twitterStatus, CancellationToken cancellationToken)
        {
            var start         = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (cancellationToken.IsCancellationRequested) yield break;
                if (item.Start >= start)
                {
                    var len  = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return (FlowContentNodeType.Text, text);
                }

                yield return (item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            if (cancellationToken.IsCancellationRequested) yield break;
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

        private static Control Url(string link)
        {
            return LinkControl(
                link.TruncateWithEllipsis(25),
                App.Commands.OpenUrl,
                link,
                ContextMenu(link));
        }

        private static Control Mention(string screenName)
        {
            return LinkControl(
                "@" + screenName,
                App.Commands.SetUserProfileContext,
                screenName);
        }

        private static Control Hashtag(string text)
        {
            return LinkControl(
                "#" + text,
                App.Commands.OpenUrl,
                $"https://twitter.com/hashtag/{text}");
        }

        private static Control LinkControl(
            string text,
            ICommand command,
            object commandParamater,
            ContextMenu? contextMenu = null)
        {
            var button = new Button();
            button.Classes.Add("inline");
            button.Command          = command;
            button.CommandParameter = commandParamater;
            button.ContextMenu      = contextMenu;
            
            var textBlock = new TextBlock();
            textBlock.Classes.Add("hyperlink");
            textBlock.Tag = commandParamater; // LongUrlService needs this
            textBlock.SetValue(ToolTip.TipProperty, commandParamater);
            textBlock.SetValue(ToolTip.ShowDelayProperty, 400);

            if (command == App.Commands.OpenUrl)
            {
                var settings = App.ServiceProvider.GetService<ISettings>();
                var binding = new Binding {
                    Source             = settings,
                    Path               = nameof(settings.ShortLinks),
                    Mode               = BindingMode.OneWay,
                    Converter          = new ShortLinkConverter(),
                    ConverterParameter = text.HtmlDecode()
                };
                textBlock.Bind(TextBlock.TextProperty, binding);
            }
            else
            {
                textBlock.Text = text.HtmlDecode();
            }

            button.Content = textBlock;
            return button;
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
                Icon             = new TextBlock { Classes = new Classes("symbol1", "padleft"), Text = App.GetString("TwitterSymbol") }
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