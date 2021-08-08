using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Services;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    internal class HomeTimelineView : UserControl
    {
        public HomeTimelineView()
        {
            InitializeComponent();
            DataContext = Bootstrapper.ServiceProvider.GetService<HomeTimelineViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}