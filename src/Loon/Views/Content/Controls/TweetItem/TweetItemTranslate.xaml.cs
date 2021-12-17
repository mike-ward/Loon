using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.Extensions;
using Loon.Services;
using Loon.ViewModels;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemTranslate : UserControl
    {
        public TweetItemTranslate()
        {
            AvaloniaXamlLoader.Load(this);
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void OnTranslateClick(object? _, RoutedEventArgs __)
        {
            try
            {
                if (DataContext is not null)
                {
                    var tweet    = (TwitterStatus)DataContext;
                    var fromLang = tweet.Language ?? "und";
                    var toLang   = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                    tweet.TranslatedText = App.GetString("translate-text-working");

                    tweet.TranslatedText = await TranslateService.Translate(
                            tweet.FullText,
                            fromLang,
                            toLang,
                            (App.MainWindow.DataContext as MainWindowViewModel)?.Settings.TranslateApiKey)
                        .ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            SetVisibility();
        }

#pragma warning disable 8631 // Nullability of type argument 'T' must match constraint type 'object' constraint in order to use it as parameter 'T'

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property.Name.IsEqualTo(nameof(Tag)))
            {
                SetVisibility();
            }
        }

        private void SetVisibility()
        {
            IsVisible = DataContext is TwitterStatus status
                && status.Language.IsNotEqualToIgnoreCase(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                && status.Language.IsNotEqualToIgnoreCase("und")
                && status.FullText.IsNotNullOrWhiteSpace()
                && Tag is bool and false;
        }
    }
}