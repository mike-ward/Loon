using Avalonia;

namespace Loon.Interfaces
{
    public interface IWindow
    {
        string Title { get; set; }
        PixelPoint Position { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}