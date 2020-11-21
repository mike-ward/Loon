using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TweetX.ViewModels.Content.SignIn;

namespace TweetX.Views.Content.SignIn
{
    internal sealed class GetPinView : UserControl
    {
        private GetPinViewModel ViewModel => (GetPinViewModel)DataContext!;

        public GetPinView()
        {
            InitializeComponent();
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

        public void GetPinClickAsync(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // for rent
        }

        public void SignInClickAsync(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // for rent
        }

        public void BackButton(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            // for rent
        }
    }
}