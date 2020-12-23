using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines
{
    public class HomeTimelineView : UserControl
    {
        public HomeTimelineView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}