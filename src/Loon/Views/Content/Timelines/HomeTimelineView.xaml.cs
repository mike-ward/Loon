using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public sealed class HomeTimelineView : UserControl
    {
        public HomeTimelineView()
        {
            DataContext = App.ServiceProvider.GetService<HomeTimelineViewModel>();
            AvaloniaXamlLoader.Load(this);
        }
    }
}