﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content;

namespace Loon.Views.Content
{
    public sealed class GetPinView : UserControl
    {
        public GetPinView()
        {
            DataContext = App.ServiceProvider.GetService<GetPinViewModel>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<TextBox>("PinTextBox")
              ?.AddHandler(
                    TextInputEvent,
                    (_, e) => e.Text = e.Text?.ToUpperInvariant(),
                    RoutingStrategies.Tunnel);
        }
    }
}