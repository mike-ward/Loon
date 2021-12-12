using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls
{
    public class Hyperlink : UserControl
    {
        public Hyperlink()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<Hyperlink, string>(nameof(Text));

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly StyledProperty<string> LinkProperty = AvaloniaProperty.Register<Hyperlink, string>(nameof(Link));

        public string Link
        {
            get => GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        public void OnClick()
        {
            Services.OpenUrlService.Open(Link);
        }
    }
}