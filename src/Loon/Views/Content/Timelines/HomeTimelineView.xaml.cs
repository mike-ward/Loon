using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    internal class HomeTimelineView : UserControl
    {
        public HomeTimelineView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<HomeTimelineViewModel>();
        }
    }
}