using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Avalonia.Controls.Documents;
using Loon.Models;
using Twitter.Models;

namespace Loon.Services
{
    internal static class FlowContentService
    {
        // Best I can do until Avalonia supports Inlines
        public static IEnumerable<Inline> FlowContentInlines(TwitterStatus twitterStatus, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) yield break;

            if (twitterStatus.FlowContent is null)
            {
                var content = FlowContentNodes(twitterStatus, cancellationToken).ToArray();
                if (cancellationToken.IsCancellationRequested) yield break;
                twitterStatus.FlowContent = content;
            }

            foreach (var (nodeType, text) in ((FlowContentNodeType, string )[])twitterStatus.FlowContent)
            {
                if (cancellationToken.IsCancellationRequested) yield break;

                switch (nodeType)
                {
                    case FlowContentNodeType.Text:
                        foreach (var inline in InlinesService.Runs(text)) yield return inline;
                        continue;
                    case FlowContentNodeType.Url:
                        yield return InlinesService.Url(text);
                        break;
                    case FlowContentNodeType.Mention:
                        yield return InlinesService.Mention(text);
                        break;
                    case FlowContentNodeType.HashTag:
                        yield return InlinesService.Hashtag(text);
                        break;
                    case FlowContentNodeType.Media:
                        continue;
                    default:
                        throw new ConstraintException("invalid FlowContentNodeType");
                }
            }
        }

        private static IEnumerable<(FlowContentNodeType, string)> FlowContentNodes(TwitterStatus twitterStatus, CancellationToken cancellationToken)
        {
            var start         = 0;
            var twitterString = new TwitterString(twitterStatus.FullText ?? twitterStatus.Text ?? string.Empty);

            foreach (var item in FlowControlItems(twitterStatus.Entities ?? new Entities()))
            {
                if (cancellationToken.IsCancellationRequested) yield break;
                if (item.Start >= start)
                {
                    var len  = item.Start - start;
                    var text = twitterString.Substring(start, len);
                    yield return (FlowContentNodeType.Text, text);
                }

                yield return (item.FlowContentNodeType, item.Text);
                start = item.End;
            }

            if (cancellationToken.IsCancellationRequested) yield break;
            yield return (FlowContentNodeType.Text, twitterString.Substring(start));
        }

        private static IEnumerable<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)> FlowControlItems(Entities entities)
        {
            var urls = entities
                          .Urls
                         ?.Select(url =>
                           (
                               FlowContentNodeType: FlowContentNodeType.Url,
                               Text: url.ExpandedUrl,
                               Start: url.Indices[0],
                               End: url.Indices[1]
                           ))
                    ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var mentions = entities
                              .Mentions
                             ?.Select(mention =>
                               (
                                   FlowContentNodeType: FlowContentNodeType.Mention,
                                   Text: mention.ScreenName,
                                   Start: mention.Indices[0],
                                   End: mention.Indices[1]
                               ))
                        ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var hashTags = entities
                              .HashTags
                             ?.Select(hashtag =>
                               (
                                   FlowContentNodeType: FlowContentNodeType.HashTag,
                                   hashtag.Text,
                                   Start: hashtag.Indices[0],
                                   End: hashtag.Indices[1]
                               ))
                        ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            var media = entities.Media
                          ?.Select(mediaItem =>
                            (
                                FlowContentNodeType: FlowContentNodeType.Media,
                                Text: mediaItem.Url,
                                Start: mediaItem.Indices[0],
                                End: mediaItem.Indices[1]
                            ))
                     ?? Enumerable.Empty<(FlowContentNodeType FlowContentNodeType, string Text, int Start, int End)>();

            return urls
               .Concat(mentions)
               .Concat(hashTags)
               .Concat(media)
               .OrderBy(o => o.Start);
        }
    }
}