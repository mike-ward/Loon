using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class HomeTimelineView : UserControl
    {
        public HomeTimelineView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<HomeTimelineViewModel>();
        }
    }
}