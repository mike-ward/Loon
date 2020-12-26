using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Loon.Commands;
using Loon.Interfaces;

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
                await SetFocus(tabItem).ConfigureAwait(false);
                TabGoBackCommand.Command.LastSelectedTab = e.RemovedItems.Cast<TabItem>().FirstOrDefault();
            }
        }

        private static async ValueTask SetFocus(TabItem? tabItem)
        {
            if (tabItem?.Content is ISetFocus view)
            {
                await Task.Delay(500).ConfigureAwait(true); // too soon and focus won't work
                view.SetFocus();
            }
        }
    }
}