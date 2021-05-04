using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Services;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class LikesTimelineView : UserControl
    {
        public LikesTimelineView()
        {
            InitializeComponent();
            DataContext = Bootstrapper.ServiceProvider.GetService<LikesTimelineViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}