using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Views.Content.Timelines.Search;

namespace Loon.Views.Content
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (sender is TabControl tabControl)
            {
                var tabItem = tabControl.SelectedItem as TabItem;
                var searchView = tabItem?.Content as SearchView;

                if (searchView is not null)
                {
                    await searchView.SetFocus();
                }
            }
        }
    }
}