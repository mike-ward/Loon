using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Write
{
    internal class WriteEditSection : UserControl
    {
        public static readonly string WriteTextBoxName = "WriteTextBox";

        public WriteEditSection()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}