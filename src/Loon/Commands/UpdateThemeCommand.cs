using System;
using Avalonia.Markup.Xaml.Styling;

#pragma warning disable S1075 // URIs should not be hardcoded

namespace Loon.Commands
{
    public class UpdateThemeCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            // This is slated to change in future release of Avalonia
            var styles = new StyleInclude(new Uri("resm:Styles"))
            {
                Source = parameter is bool val && val
                    ? new Uri("avares://Avalonia.Themes.Default/Accents/BaseLight.xaml")
                    : new Uri("avares://Avalonia.Themes.Default/Accents/BaseDark.xaml")
            };

            App.Current.Styles[1] = styles;
        }
    }
}