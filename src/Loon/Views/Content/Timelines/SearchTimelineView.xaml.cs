using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Jab;
using Loon.Services;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class SearchTimelineView : UserControl
    {
        public static readonly string SearchTextBoxName = "SearchTextBox";

        public SearchTimelineView()
        {
            InitializeComponent();
            DataContext = Bootstrapper.ServiceProvider.GetService<SearchTimelineViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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