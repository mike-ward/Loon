using System;

namespace Loon.Models
{
    public record WindowLocation
    {
        private int x;
        private int y;
        private double width;
        private double height;

        public int X { get => x; set => x = Math.Max(0, value); }
        public int Y { get => y; set => y = Math.Max(0, value); }
        public double Width { get => width; set => width = Math.Max(100, value); }
        public double Height { get => height; set => height = Math.Max(100, value); }
    }
}