﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Extensions;
using Loon.Services;
using Loon.Views.Content.Timelines;

namespace Loon.Views.Content
{
    public sealed class MainView : UserControl
    {
        private int previousIndex;

        // ReSharper disable once ConvertToConstant.Global
        // Referenced in XAML, can't be constant
        public static readonly string TabControlName = "TabControl";

        public MainView()
        {
            AvaloniaXamlLoader.Load(this);
            PubSubs.OpenWriteTab.Subscribe(OpenWriteTabHandler);
            PubSubs.OpenPreviousTab.Subscribe(OpenPreviousTabHandler);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            Focus();
        }

        public void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name.IsEqualTo(nameof(TabControl.SelectedIndex)) &&
                e.OldValue is int old)
            {
                previousIndex = old;
            }
        }

        public void OnWriteTabClicked(object? sender, PointerPressedEventArgs e)
        {
            App.Commands.OpenWriteTab.Execute(null); // wanted side-effect; clear replyTo in write tab.
        }

        private void OpenPreviousTabHandler(object? _)
        {
            if (this.FindControl<TabControl>(TabControlName) is { } tabControl)
            {
                tabControl.SelectedIndex = previousIndex;
            }
        }

        private void OpenWriteTabHandler(object? _)
        {
            if (this.FindControl<TabControl>(TabControlName) is { } tabControl)
            {
                tabControl.SelectedIndex = tabControl.ItemCount - 1;
            }
        }

        private void TabItem_OnPointerPressed(object? sender, PointerPressedEventArgs _)
        {
            var tabItem      = sender as TabItem;
            var content      = (tabItem?.Content as UserControl)?.Content as IVisual;
            var timelineView = content as TimelineView ?? content.FindAncestorOfType<TimelineView>();
            var scrollViewer = timelineView?.FindDescendantOfType<ScrollViewer>();
            scrollViewer?.ScrollToHome();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyModifiers != KeyModifiers.Alt) return;
            const double delta = 0.1;

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (e.Key)
            {
                case Key.Add:
                case Key.OemPlus:
                    App.Settings.FontSize += delta;
                    e.Handled             =  true;
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    App.Settings.FontSize -= delta;
                    e.Handled             =  true;
                    break;
            }
        }
    }
}