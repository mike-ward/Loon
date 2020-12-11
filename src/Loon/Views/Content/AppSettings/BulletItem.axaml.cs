using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.AppSettings
{
    public class BulletItem : UserControl
    {
        public static readonly StyledProperty<string> BulletTextProperty = AvaloniaProperty.Register<BulletItem, string>(nameof(BulletText));

        public string BulletText
        {
            get { return GetValue(BulletTextProperty); }
            set { SetValue(BulletTextProperty, value); }
        }

        public BulletItem()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}