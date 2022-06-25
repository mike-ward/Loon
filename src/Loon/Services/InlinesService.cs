using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;
using Loon.Converters;
using Loon.Extensions;

namespace Loon.Services
{
    public static class InlinesService
    {
        private static readonly IValueConverter LinkConverter = new ShortLinkConverter();

        public static TextBlock Run(string text)
        {
            var textBlock = new TextBlock();
            textBlock.Classes.Add("normal");
            textBlock.Text                = text.HtmlDecode();
            textBlock.VerticalAlignment   = VerticalAlignment.Top;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            return textBlock;
        }

        public static Control Url(string link)
        {
            return LinkControl(
                link.TruncateWithEllipsis(25),
                App.Commands.OpenUrl,
                link,
                ContextMenu(link));
        }

        public static Control Mention(string screenName)
        {
            return LinkControl(
                "@" + screenName,
                App.Commands.SetUserProfileContext,
                screenName);
        }

        public static Control Hashtag(string text)
        {
            return LinkControl(
                "#" + text,
                App.Commands.OpenUrl,
                $"https://twitter.com/hashtag/{text}");
        }

        private static Control LinkControl(
            string       text,
            ICommand     command,
            object       commandParameter,
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

            if (command == App.Commands.OpenUrl 
             && text.StartsWith('#') is false) // don't convert hashtag links
            {
                var binding = new Binding
                {
                    Source             = App.Settings,
                    Path               = nameof(App.Settings.ShortLinks),
                    Mode               = BindingMode.OneWay,
                    Converter          = LinkConverter,
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