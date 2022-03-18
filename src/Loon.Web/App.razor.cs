using Avalonia.Web.Blazor;

namespace Loon.Web
{
    public partial class App
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            WebAppBuilder
               .Configure<Loon.App>()
               .SetupWithSingleViewLifetime();
        }
    }
}