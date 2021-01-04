using Avalonia.Controls;
using Loon.Interfaces;
using Loon.Services;

namespace Loon.ViewModels.Content
{
    public class MainViewModel
    {
        private int previousIndex;
        private TabControl? tabControl;

        public ISettings Settings { get; }

        public MainViewModel(ISettings settings)
        {
            Settings = settings;
            PubSubs.OpenWriteTab.Subscribe(OpenWriteTabHandler);
            PubSubs.OpenPreviousTab.Subscribe(OpenPreviousTabHandler);
        }

        public void SetPreviousIndex(int idx, TabControl? tabCtrl)
        {
            previousIndex = idx;
            tabControl = tabCtrl ?? tabControl;
        }

        private void OpenPreviousTabHandler()
        {
            if (tabControl is not null)
            {
                tabControl.SelectedIndex = previousIndex;
            }
        }

        private void OpenWriteTabHandler(object? _)
        {
            if (tabControl is not null)
            {
                tabControl.SelectedIndex = tabControl.ItemCount - 1;
            }
        }
    }
}