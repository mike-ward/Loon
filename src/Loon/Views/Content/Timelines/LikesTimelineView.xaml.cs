using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public sealed class LikesTimelineView : UserControl
    {
        public LikesTimelineView()
        {
            DataContext = App.ServiceProvider.GetService<LikesTimelineViewModel>();
            AvaloniaXamlLoader.Load(this);
        }
    }
}