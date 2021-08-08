using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.Extensions;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    internal class TweetItemTranslate : UserControl
    {
        public TweetItemTranslate()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnTranslateClick(object? _, RoutedEventArgs __)
        {
            if (DataContext is not null)
            {
                var tweet    = (TwitterStatus)DataContext;
                var fromLang = tweet.Language ?? "und";
                var toLang   = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                tweet.TranslatedText = App.GetString("translate-text-working");
                tweet.TranslatedText = await TranslateService.Translate(tweet.FullText, fromLang, toLang).ConfigureAwait(true);
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