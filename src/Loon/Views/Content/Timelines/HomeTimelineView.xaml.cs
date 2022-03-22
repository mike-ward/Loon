using System.Collections.Specialized;
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
            ((HomeTimelineViewModel)DataContext).StatusCollection.CollectionChanged += StatusCollectionOnCollectionChanged;
        }

        private void StatusCollectionOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var announce = this.Find<TextBlock>("Announce");
            announce.IsVisible = ((HomeTimelineViewModel)DataContext!).StatusCollection.Count == 0;
        }
    }
}