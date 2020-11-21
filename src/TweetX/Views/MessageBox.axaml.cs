using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace TweetX.Views
{
    internal class MessageBox : Window
    {
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

        public static Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
        {
            ContentControl? autoFocusControl = null;
            var res = MessageBoxResult.Ok;

            var msgbox = new MessageBox();
            msgbox.FindControl<TextBlock>("Title").Text = title;
            msgbox.FindControl<TextBlock>("Text").Text = text;

            var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

            if (buttons == MessageBoxButtons.Ok || buttons == MessageBoxButtons.OkCancel)
            {
                AddButton("OK", MessageBoxResult.Ok, def: true);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, def: true);
            }

            if (buttons == MessageBoxButtons.OkCancel || buttons == MessageBoxButtons.YesNoCancel)
            {
                AddButton("Cancel", MessageBoxResult.Cancel, def: true);
            }

            var tcs = new TaskCompletionSource<MessageBoxResult>();
            msgbox.Closed += delegate { tcs.TrySetResult(res); };

            if (parent is not null)
            {
                msgbox.ShowDialog(parent);
            }
            else
            {
                msgbox.Show();
            }

            autoFocusControl?.Focus();
            return tcs.Task;

            void AddButton(string caption, MessageBoxResult r, bool def = false)
            {
                var btn = new Button { Content = caption };

                btn.Click += (_, __) =>
                {
                    res = r;
                    msgbox.Close();
                };

                buttonPanel.Children.Add(btn);

                if (def)
                {
                    res = r;
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