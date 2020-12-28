using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Loon.Behaviors
{
    public class PreviousIndexBehavior : AvaloniaObject
    {
        public static readonly AttachedProperty<int> PreviousIndexProperty =
            AvaloniaProperty.RegisterAttached<PreviousIndexBehavior, TabControl, int>
                ("PreviousIndex", default, false, BindingMode.OneTime, null, UpdateIndex);

        public static void SetPreviousIndex(Control element, int value)
        {
            element.SetValue(PreviousIndexProperty, value);
        }

        public static int GetPreviousIndex(Control element)
        {
            return element.GetValue(PreviousIndexProperty);
        }

        private static int UpdateIndex(IAvaloniaObject element, int index)
        {
            if (element is TabControl tabControl)
            {
                tabControl.RemoveHandler(TabControl.SelectionChangedEvent, Handler);
                tabControl.AddHandler(TabControl.SelectionChangedEvent, Handler);
            }

            return index;

            void Handler(object? s, SelectionChangedEventArgs e)
            {
                var removedTabItem = e.RemovedItems.Cast<TabItem>().FirstOrDefault();
                if (removedTabItem?.Parent is TabControl tabControl)
                {
                    var previousIndex = tabControl.Items
                        .Cast<TabItem>()
                        .Select((item, idx) => (item, idx))
                        .First(t => removedTabItem == t.item)
                        .idx;

                    SetPreviousIndex(tabControl, previousIndex);
                }
            }
        }
    }
}