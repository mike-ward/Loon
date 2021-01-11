using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemRelated : UserControl
    {
        public TweetItemRelated()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

#pragma warning disable CA1822 // (used in XAML) Mark members as static
#pragma warning disable IDE0051 // (used in XAML) Remove unused private members
#pragma warning disable RCS1213 // (used in XAML) Remove unused member declaration.
#pragma warning disable S1144 // (used in XAML) Unused private types or members should be removedremoved

        private void OpenUrl(string link)
        {
            Services.OpenUrlService.Open(link);
        }
    }
}