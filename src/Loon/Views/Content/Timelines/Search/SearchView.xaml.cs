using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines.Search
{
    public class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async ValueTask SetFocus()
        {
            await Task.Delay(1000).ConfigureAwait(true);
            var textBox = this.FindControl<TextBox>("SearchTextBox");
            textBox?.Focus();
        }

        public async void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) &&
                sender is TextBox textBox &&
                DataContext is SearchTimelineViewModel vm)
            {
                await vm.OnSearch(textBox.Text);
            }
        }
    }
}