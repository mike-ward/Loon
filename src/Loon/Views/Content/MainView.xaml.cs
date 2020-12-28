using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Write;
using Loon.Views.Content.Write;

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

        public void OnSelectionChanged(object? s, SelectionChangedEventArgs e)
        {
            if (this.FindLogicalDescendantOfType<WriteView>() is WriteView writeView &&
                writeView.DataContext is WriteViewModel writeViewModel)
            {
                writeViewModel.Reset();
            }
        }
    }
}