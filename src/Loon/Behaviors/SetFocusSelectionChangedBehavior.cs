using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Loon.Behaviors
{
    public class SetFocusSelectionChangedBehavior : AvaloniaObject
    {
        public static readonly AttachedProperty<string> NameProperty =
            AvaloniaProperty.RegisterAttached<SetFocusSelectionChangedBehavior, TabControl, string>
                ("Name", string.Empty, false, BindingMode.OneTime, null, FocusType);

        public static void SetName(Control element, string value)
        {
            element.SetValue(NameProperty, value);
        }

        public static string GetName(Control element)
        {
            return element.GetValue(NameProperty);
        }

        private static string FocusType(IAvaloniaObject element, string name)
        {
            if (element is IControl { Parent: TabControl tabControl })
            {
                tabControl.RemoveHandler(SelectingItemsControl.SelectionChangedEvent, Handler);
                tabControl.AddHandler(SelectingItemsControl.SelectionChangedEvent, Handler);
            }

            return name;

            async void Handler(object? s, SelectionChangedEventArgs e)
            {
                if (e.AddedItems.Cast<IControl>().FirstOrDefault() is TabItem tab)
                {
                    var timeline = tab.Content as ILogical;

                    foreach (var descendant in timeline.GetLogicalDescendants())
                    {
                        if (descendant.FindNameScope().Find(name) is IInputElement control)
                        {
                            await Task.Delay(500).ConfigureAwait(true); // too soon and focus won't work
                            control.Focus();
                            break;
                        }
                    }
                }
            }
        }
    }
}