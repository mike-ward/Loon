using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Loon.Extensions;
using Loon.Services;
using Twitter.Models;

namespace Loon.Views.Content.Controls.TweetItem
{
    public class TweetItemTranslate : UserControl
    {
        public TweetItemTranslate()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void OnTranslateClick(object? _, RoutedEventArgs __)
        {
            if (DataContext is not null)
            {
                var tweet = (TwitterStatus)DataContext;
                var lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                tweet.TranslatedText = App.GetString("translate-text-working");
                tweet.TranslatedText = await TranslateService.Translate(tweet.FullText, lang).ConfigureAwait(true);
            }
        }

        protected override void OnDataContextChanged(System.EventArgs e)
        {
            IsVisible = DataContext is TwitterStatus status
                && status.Language.IsNotEqualToIgnoreCase(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                && status.FullText.IsPopulated();
        }
    }
}