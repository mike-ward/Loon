﻿using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Loon.Converters;
using Loon.Extensions;

namespace Loon.Services
{
    public static class InlinesService
    {
        private static readonly IValueConverter ShortLinkConverter = new ShortLinkConverter();

        public static IEnumerable<Inline> Runs(string text)
        {
            var builder = new StringBuilder();

            foreach (var c in text)
            {
                if (c != '\n')
                {
                    builder.Append(c);
                    continue;
                }

                var t = builder.ToString();
                if (t.Length > 0) yield return Run(t);
                yield return new LineBreak();
                builder.Clear();
            }

            var remaining = builder.ToString();
            if (remaining.Length > 0) yield return Run(remaining);
        }

        private static Run Run(string text)
        {
            var run = new Run();
            run.Classes.Add("normal");
            run.Text = text.HtmlDecode();
            return run;
        }

        public static InlineUIContainer Url(string link)
        {
            const int maxLength  = 25;
            var       linkAsText = link.TruncateWithEllipsis(maxLength);

            return LinkControl(
                linkAsText,
                App.Commands.OpenUrl,
                link,
                ShortLinkConverterBinding(linkAsText),
                ContextMenu(link));
        }

        public static InlineUIContainer Mention(string screenName)
        {
            return LinkControl(
                "@" + screenName,
                App.Commands.SetUserProfileContext,
                screenName);
        }

        public static InlineUIContainer Hashtag(string text)
        {
            return LinkControl(
                "#" + text,
                App.Commands.OpenUrl,
                $"https://twitter.com/hashtag/{text}");
        }

        private static InlineUIContainer LinkControl(
            string       text,
            ICommand     command,
            object       commandParameter,
            Binding?     binding     = null,
            ContextMenu? contextMenu = null)
        {
            var button = new Button();
            button.Classes.Add("inline");
            button.Command             = command;
            button.CommandParameter    = commandParameter;
            button.ContextMenu         = contextMenu;
            button.VerticalAlignment   = VerticalAlignment.Top;
            button.HorizontalAlignment = HorizontalAlignment.Left;

            var textBlock = new TextBlock();
            textBlock.Classes.Add("hyperlink");
            textBlock.Tag = commandParameter; // LongUrlService needs this
            textBlock.SetValue(ToolTip.TipProperty, commandParameter);
            textBlock.SetValue(ToolTip.ShowDelayProperty, 400);

            if (binding is not null)
            {
                textBlock.Bind(TextBlock.TextProperty, binding);
            }
            else
            {
                textBlock.Text = text.HtmlDecode();
            }

            button.Content = textBlock;
            return new InlineUIContainer(button);
        }

        private static Binding ShortLinkConverterBinding(string text)
        {
            return new Binding
            {
                Source             = App.Settings,
                Path               = nameof(App.Settings.ShortLinks),
                Mode               = BindingMode.OneWay,
                Converter          = ShortLinkConverter,
                ConverterParameter = text.HtmlDecode()
            };
        }

        private static ContextMenu ContextMenu(string link)
        {
            var copyLinkAddress = new MenuItem
            {
                Header           = App.GetString("copy-link-address"),
                Command          = App.Commands.CopyToClipboard,
                CommandParameter = link
            };

            var emailLinkAddress = new MenuItem
            {
                Header           = App.GetString("email-link"),
                CommandParameter = link,
                Icon             = new TextBlock { Classes = new Classes("symbol", "padleft"), Text = App.GetString("MailSymbol") }
            };

            var shareLinkTwitter = new MenuItem
            {
                Header           = App.GetString("share-link-twitter"),
                CommandParameter = link,
                Icon             = new TextBlock { Classes = new Classes("symbol1", "padleft"), Text = App.GetString("TwitterSymbol") }
            };

            return new ContextMenu
            {
                HorizontalOffset = 15,
                VerticalOffset   = 10,
                Items = new[]
                {
                    copyLinkAddress,
                    emailLinkAddress,
                    shareLinkTwitter
                }
            };
        }
    }
}