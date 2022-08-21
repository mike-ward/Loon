using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Loon.Views.Content.Controls
{
    public sealed class MessageBox : Window
    {
        // ReSharper disable ConvertToConstant.Global

        public static readonly string TextName    = "Text";
        public static readonly string TitleName   = "Title";
        public static readonly string ButtonsName = "Buttons";

        // ReSharper restore ConvertToConstant.Global

        public enum MessageBoxButtons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum MessageBoxResult
        {
            Ok,
            Cancel,
            Yes,
            No
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public MessageBox()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static async Task<MessageBoxResult> Show(string text, MessageBoxButtons buttons, PixelPoint? pixelPoint = null)
        {
            ContentControl? autoFocusControl = null;
            var             messageBoxResult = MessageBoxResult.Ok;

            var msgbox = new MessageBox();

            if (pixelPoint is not null)
            {
                msgbox.WindowStartupLocation = WindowStartupLocation.Manual;
                msgbox.Position              = pixelPoint.Value;
            }

            msgbox.FindControl<TextBlock>(TitleName)!.Text = App.GetString("title");
            msgbox.FindControl<TextBlock>(TextName)!.Text  = text;

            var buttonPanel = msgbox.FindControl<StackPanel>(ButtonsName);

            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (buttons is MessageBoxButtons.Ok or MessageBoxButtons.OkCancel)
            {
                AddButton("OK", MessageBoxResult.Ok, true);
            }

            if (buttons is MessageBoxButtons.YesNo or MessageBoxButtons.YesNoCancel)
            {
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
            }

            if (buttons is MessageBoxButtons.OkCancel or MessageBoxButtons.YesNoCancel)
            {
                AddButton("Cancel", MessageBoxResult.Cancel, true);
            }

            await msgbox.ShowDialog(App.MainWindow);
            autoFocusControl?.Focus();
            return messageBoxResult;

            void AddButton(string caption, MessageBoxResult mbr, bool isDefaultButton = false)
            {
                var btn = new Button { Content = caption, MinWidth = 50 };

                btn.Click += delegate
                {
                    messageBoxResult = mbr;
                    msgbox.Close();
                };

                buttonPanel?.Children.Add(btn);

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

        private void OnPointerPressed(object? _, PointerPressedEventArgs e)
        {
            BeginMoveDrag(e);
        }
    }
}