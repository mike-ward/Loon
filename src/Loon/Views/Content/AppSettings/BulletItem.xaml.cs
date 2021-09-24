using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    internal class BulletItem : UserControl
    {
        public static readonly StyledProperty<string> BulletTextProperty = AvaloniaProperty.Register<BulletItem, string>(nameof(BulletText));

        public string BulletText
        {
            get => GetValue(BulletTextProperty);
            set => SetValue(BulletTextProperty, value);
        }

        public BulletItem()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}