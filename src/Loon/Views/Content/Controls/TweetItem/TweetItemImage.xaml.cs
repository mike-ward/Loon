﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Loon.Interfaces;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemImage : UserControl
    {
        private CancellationToken cancellationToken = CancellationToken.None;

        public TweetItemImage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            if (this.FindLogicalAncestorOfType<ICancellationTokeSourceProvider>() is { } cancellationTokeSourceProvider)
            {
                cancellationToken = cancellationTokeSourceProvider.CancellationTokenSource.Token;
            }

            base.OnDataContextChanged(e);
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private async void LoadMediaAsync(object? sender, EventArgs _)
        {
            var token = cancellationToken; // make a copy
            if (token.IsCancellationRequested) return;
            
            if (sender is Image { DataContext: Media media } image)
            {
                try
                {
                    image.Source = null;
                    var imageSource = await ImageService.GetImageAsync(media.MediaUrl, token);
                    if (token.IsCancellationRequested) return;
                    image.Source ??= imageSource;
                }
                catch (Exception ex)
                {
                    TraceService.Message(ex.Message);
                }
            }
        }

        private void OpenInViewer(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed &&
                sender is Grid grid &&
                grid.Children[0] is Image image)
            {
                ImageService.OpenInViewer(image);
            }
        }
    }
}