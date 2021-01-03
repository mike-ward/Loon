using Avalonia.Controls;
using Loon.Extensions;
using Loon.Interfaces;
using Loon.Models;
using Loon.ViewModels.Content.Write;

namespace Loon.ViewModels.Content
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private int previousIndex;

        public TabControl? TabControl { get; set; }

        public const string OpenPreviousTabMessage = "previous-tab-message";
        private IPubSubService PubSubService { get; }

        public MainViewModel(IPubSubService pubSubService)
        {
            PubSubService = pubSubService;
            PubSubService.PubSubRaised += OpenPreviousTabHandler;
            PubSubService.PubSubRaised += OpenWriteTabHandler;
        }

        public void SetPreviousIndex(int idx, TabControl? tabControl)
        {
            previousIndex = idx;
            if (TabControl is null) { TabControl = tabControl; }
        }

        private void OpenPreviousTabHandler(object? sender, PubSubEventArgs e)
        {
            if (e.Message.IsEqualTo(OpenPreviousTabMessage) && TabControl is not null)
            {
                TabControl.SelectedIndex = previousIndex;
            }
        }

        private void OpenWriteTabHandler(object? sender, PubSubEventArgs e)
        {
            if (e.Message.IsEqualTo(WriteViewModel.OpenWriteTabMessage) && TabControl is not null)
            {
                TabControl.SelectedIndex = TabControl.ItemCount - 1;
            }
        }
    }
}