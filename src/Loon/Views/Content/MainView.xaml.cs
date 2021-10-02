using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.Extensions;
using Loon.Services;

namespace Loon.Views.Content
{
    internal class MainView : UserControl
    {
        private                int    previousIndex;
        public static readonly string TabControlName = "TabControl";

        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            PubSubs.OpenWriteTab.Subscribe(OpenWriteTabHandler);
            PubSubs.OpenPreviousTab.Subscribe(OpenPreviousTabHandler);
        }

        public void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.IsEqualTo(nameof(TabControl.SelectedIndex)) &&
                e.OldValue is int old)
            {
                previousIndex = old;
            }
        }

        public void OnWriteTabClicked(object? sender, PointerPressedEventArgs e)
        {
            App.Commands.OpenWriteTab.Execute(null); // wanted side-effect; clear replyTo in write tab.
        }

        private void OpenPreviousTabHandler(object? _)
        {
            if (this.FindControl<TabControl>(TabControlName) is { } tabControl)
            {
                tabControl.SelectedIndex = previousIndex;
            }
        }

        private void OpenWriteTabHandler(object? _)
        {
            if (this.FindControl<TabControl>(TabControlName) is { } tabControl)
            {
                tabControl.SelectedIndex = tabControl.ItemCount - 1;
            }
        }
    }
}