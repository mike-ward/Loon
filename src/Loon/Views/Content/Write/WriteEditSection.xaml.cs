using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Write
{
    public sealed class WriteEditSection : UserControl
    {
        public static readonly string WriteTextBoxName = "WriteTextBox";

        public WriteEditSection()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}