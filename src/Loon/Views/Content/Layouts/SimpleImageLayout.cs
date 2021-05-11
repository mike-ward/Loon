﻿using Avalonia;
using Avalonia.Controls;

namespace Loon.Views.Content.Layouts
{
    /// <summary>
    /// Simple image panel designed specifically to display the 1-4 images
    /// that twitter returns. Not generic.
    /// </summary>
    internal class SimpleImageLayout : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(
                availableSize.Width,
                Children.Count > 0
                    ? Models.Constants.ImagePanelHeight
                    : 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double w;
            double h;

            switch (Children.Count)
            {
                // fill entire panel
                case 1:
                    w = finalSize.Width;
                    h = finalSize.Height;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    break;

                // Two columns, side by side
                case 2:
                    w = finalSize.Width / 2;
                    h = finalSize.Height;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    break;

                // Two columns, Two rows, first column spans both rows
                case 3:
                    w = finalSize.Width / 2;
                    h = finalSize.Height / 2;
                    Children[0].Arrange(new Rect(0, 0, w, finalSize.Height));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    Children[2].Arrange(new Rect(w, h, w, h));
                    break;

                // Two rows, two columns
                case 4:
                    w = finalSize.Width / 2;
                    h = finalSize.Height / 2;
                    Children[0].Arrange(new Rect(0, 0, w, h));
                    Children[1].Arrange(new Rect(w, 0, w, h));
                    Children[2].Arrange(new Rect(0, h, w, h));
                    Children[3].Arrange(new Rect(w, h, w, h));
                    break;
            }

            return finalSize;
        }
    }
}