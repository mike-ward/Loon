using System.Diagnostics.CodeAnalysis;
using Avalonia.Input;

namespace Loon.Services
{
    public static class KeyboardService
    {
        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
        public static void AppKeyDownHandler(KeyEventArgs e)
        {
            const double delta = 0.1;

            if (e.KeyModifiers == KeyModifiers.Alt)
            {
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
}