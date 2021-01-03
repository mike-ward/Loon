using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Extensions;
using Loon.ViewModels.Content;

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

        public void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.IsEqualTo(nameof(TabControl.SelectedIndex)) &&
                DataContext is MainViewModel vm &&
                e.OldValue is int old)
            {
                vm.SetPreviousIndex(old, sender as TabControl);
            }
        }

        public void OnWriteTabClicked(object? sender, PointerPressedEventArgs e)
        {
            App.Commands.OpenWriteTab.Execute(null); // wanted side-effect; clear replyTo in write tab.
        }
    }
}