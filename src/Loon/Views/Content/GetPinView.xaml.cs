using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content;

namespace Loon.Views.Content
{
    public class GetPinView : UserControl
    {
        public GetPinView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<GetPinViewModel>();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<TextBox>("PinTextBox")
                .AddHandler(
                    TextInputEvent,
                    (_, e) => e.Text = e.Text?.ToUpperInvariant(),
                    RoutingStrategies.Tunnel);
        }
    }
}