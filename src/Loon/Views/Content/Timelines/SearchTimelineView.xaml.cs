using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class SearchTimelineView : UserControl, ISetFocus
    {
        public SearchTimelineView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void SetFocus()
        {
            var textBox = this.FindControl<TextBox>("SearchTextBox");
            textBox?.Focus();
        }

        public async void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) &&
                sender is TextBox textBox &&
                DataContext is SearchTimelineViewModel vm)
            {
                await vm.OnSearch(textBox.Text).ConfigureAwait(false);
            }
        }
    }
}