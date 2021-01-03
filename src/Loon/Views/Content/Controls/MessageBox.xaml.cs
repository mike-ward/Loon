using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls
{
    public class MessageBox : Window
    {
        public static readonly string TextName = "Text";
        public static readonly string TitleName = "Title";
        public static readonly string ButtonsName = "Buttons";

        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel,
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No,
        }

        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static async Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
        {
            ContentControl? autoFocusControl = null;
            var messageBoxResult = MessageBoxResult.Ok;

            var msgbox = new MessageBox();
            msgbox.FindControl<TextBlock>(TitleName).Text = title;
            msgbox.FindControl<TextBlock>(TextName).Text = text;

            var buttonPanel = msgbox.FindControl<StackPanel>(ButtonsName);

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            {
                AddButton("OK", MessageBoxResult.Ok, isDefaultButton: true);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, isDefaultButton: true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Cancel", MessageBoxResult.Cancel, isDefaultButton: true);
            }

            await msgbox.ShowDialog(parent).ConfigureAwait(false);

            autoFocusControl?.Focus();
            return messageBoxResult;

            void AddButton(string caption, MessageBoxResult mbr, bool isDefaultButton = false)
            {
                var btn = new Button { Content = caption };

                btn.Click += delegate
                {
                    messageBoxResult = mbr;
                    msgbox.Close();
                };

                buttonPanel.Children.Add(btn);

                if (isDefaultButton)
                {
                    messageBoxResult = mbr;
                    autoFocusControl = btn;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
            }

            base.OnKeyDown(e);
        }
    }
}