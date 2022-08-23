using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Loon.Models;
using Loon.Services;
using Loon.ViewModels.Content.Timelines;
using Loon.Views.Content.AppSettings;
using Loon.Views.Content.Timelines;
using Loon.Views.Content.Write;

namespace Loon.Views.Content;

public class NavTabs : UserControl
{
    private readonly Grid      tabs;
    private readonly TextBlock homeTab;
    private readonly TextBlock likesTab;
    private readonly TextBlock searchTab;
    private readonly TextBlock settingsTab;
    private readonly TextBlock writeTab;

    public readonly  HomeTimelineView   homeView     = new() { IsVisible = false };
    private readonly LikesTimelineView  likesView    = new() { IsVisible = false };
    private readonly SearchTimelineView searchView   = new() { IsVisible = false };
    private readonly SettingsView       settingsView = new() { IsVisible = false };
    private readonly WriteView          writeView    = new() { IsVisible = false };

    private const string selected = "selected";

    public NavTabs()
    {
        AvaloniaXamlLoader.Load(this);

        tabs        = this.FindControl<Grid>("Tabs")!;
        homeTab     = this.FindControl<TextBlock>("HomeTab")!;
        likesTab    = this.FindControl<TextBlock>("LikesTab")!;
        searchTab   = this.FindControl<TextBlock>("SearchTab")!;
        settingsTab = this.FindControl<TextBlock>("SettingsTab")!;
        writeTab    = this.FindControl<TextBlock>("WriteTab")!;

        var binding = new Binding
        {
            Source = ((HomeTimelineViewModel)homeView.DataContext!).HomeTimeline,
            Path   = nameof(Timeline.PendingStatusesAvailable)
        };

        var pendingIndicator = this.FindControl<TextBlock>("PendingIndicator")!;
        pendingIndicator.Bind(IsVisibleProperty, binding);

        var content = this.FindControl<Grid>("Content")!;
        content.Children.Add(homeView);
        content.Children.Add(likesView);
        content.Children.Add(searchView);
        content.Children.Add(settingsView);
        content.Children.Add(writeView);

        Select(homeView, homeTab);
        PubSubs.OpenWriteTab.Subscribe(_ => Select(writeView, writeTab));
    }

    private void HomePressed(object? _, PointerPressedEventArgs __) => Select(homeView, homeTab);
    private void LikePressed(object? _, PointerPressedEventArgs __) => Select(likesView, likesTab);
    private void SearchPressed(object? _, PointerPressedEventArgs __) => Select(searchView, searchTab);
    private void SettingsPressed(object? _, PointerPressedEventArgs __) => Select(settingsView, settingsTab);
    private void WritePressed(object? _, PointerPressedEventArgs __) => Select(writeView, writeTab);

    private void Select(IVisual view, IStyledElement tab)
    {
        HideAll();
        UnselectAll();
        tab.Classes.Add(selected);
        view.IsVisible = true;
    }

    private void HideAll()
    {
        homeView.IsVisible     = false;
        likesView.IsVisible    = false;
        searchView.IsVisible   = false;
        settingsView.IsVisible = false;
        writeView.IsVisible    = false;
    }

    private void UnselectAll()
    {
        homeTab.Classes.Remove(selected);
        likesTab.Classes.Remove(selected);
        searchTab.Classes.Remove(selected);
        settingsTab.Classes.Remove(selected);
        writeTab.Classes.Remove(selected);
    }
}