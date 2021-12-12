﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Loon.ViewModels.Content.Timelines;

namespace Loon.Views.Content.Timelines
{
    public class SearchTimelineView : UserControl
    {
        // ReSharper disable once ConvertToConstant.Global (used in XAML)
        public static readonly string SearchTextBoxName = "SearchTextBox";

        public SearchTimelineView()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = App.ServiceProvider.GetService<SearchTimelineViewModel>();
        }

        public async void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Return) &&
                sender is TextBox textBox &&
                DataContext is SearchTimelineViewModel vm)
            {
                await vm.OnSearch(textBox.Text).ConfigureAwait(false);
            }
        }
    }
}