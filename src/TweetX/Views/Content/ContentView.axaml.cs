using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TweetX.ViewModels.Content;

namespace TweetX.Views.Content
{
    public class ContentView : UserControl
    {
        public ContentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            // <sn:GetPinView IsVisible="{Binding !SignedIn}" />
            // not working in Preview 6
            var vm = (ContentViewModel)DataContext!;
            var getPinView = this.FindControl<GetPinView>("GetPinView");
            PropertyChanged += delegate { getPinView.IsVisible = !vm.SignedIn; };
        }
    }
}