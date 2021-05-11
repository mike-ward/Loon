using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Timelines
{
    public class TimelineView : UserControl
    {
        public static readonly string ScrollViewerName  = "ScrollViewer";
        public static readonly string ItemsRepeaterName = "ItemsRepeater";

        public TimelineView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (e.InitialPressMouseButton == MouseButton.Right)
            {
                e.Handled = true;
                this.FindControl<ScrollViewer>(ScrollViewerName)?.ScrollToHome();
            }

            base.OnPointerReleased(e);
        }
    }
}