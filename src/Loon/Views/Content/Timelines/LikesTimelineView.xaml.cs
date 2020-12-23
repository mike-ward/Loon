using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines
{
    public class LikesTimelineView : UserControl
    {
        public LikesTimelineView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}