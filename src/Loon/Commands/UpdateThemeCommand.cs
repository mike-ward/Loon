using System;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

#pragma warning disable S1075 // URIs should not be hardcoded

namespace Loon.Commands
{
    public class UpdateThemeCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            var useLight = parameter is bool val && val;

            // This is slated to change in future release of Avalonia
            var styles = new StyleInclude(new Uri("resm:Styles")) {
                Source = useLight
                    ? new Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml")
                    : new Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml")
            };

            Application.Current.Styles[1] = styles;

            var overrides = new StyleInclude(new Uri("resm:Styles")) {
                Source = useLight
                    ? new Uri("avares://Loon/Assets/LightThemeOverrides.xaml")
                    : new Uri("avares://Loon/Assets/DarkThemeOverrides.xaml")
            };

            Application.Current.Styles[2] = overrides;
        }
    }
}