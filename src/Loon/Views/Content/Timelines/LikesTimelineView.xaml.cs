using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class LikesTimelineView : UserControl
    {
        public LikesTimelineView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<LikesTimelineViewModel>();
        }
    }
}