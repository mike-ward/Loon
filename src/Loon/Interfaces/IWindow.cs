using System;
using System.ComponentModel;
using Avalonia;

namespace Loon.Interfaces
{
    public interface IWindow
    {
        string Title { get; set; }
        PixelPoint Position { get; set; }
        double Width { get; set; }
        double Height { get; set; }

        event EventHandler<CancelEventArgs> Closing;
    }
}