using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Loon.Behaviors
{
    public sealed class SetFocusSelectionChangedBehavior : AvaloniaObject
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
            if (element is not IControl { Parent: TabControl tabControl }) return name;
            tabControl.RemoveHandler(SelectingItemsControl.SelectionChangedEvent, Handler);
            tabControl.AddHandler(SelectingItemsControl.SelectionChangedEvent, Handler);
            return name;

            async void Handler(object? s, SelectionChangedEventArgs e)
            {
                if (e.AddedItems.Count == 0 || e.AddedItems[0] is not TabItem tab) return;
                if (tab.Content is not ILogical timeline) return;

                foreach (var descendant in timeline.GetLogicalDescendants())
                {
                    if (descendant.FindNameScope() is { } ctl && ctl.Find(name) is InputElement control)
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