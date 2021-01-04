using System;
using System.Collections.Concurrent;
using System.Linq;
using Loon.Extensions;

namespace Loon.Services
{
    public static class PubSubService
    {
        private static readonly ConcurrentBag<(string message, Action<object?> handler)> subscribers = new();

        public const string AddStatusMessage = "add-status-message";
        public const string OpenWriteTabMessage = nameof(OpenWriteTabMessage);
        public const string OpenPreviousTabMessage = nameof(OpenPreviousTabMessage);
        public const string SetUserProfileContextMessage = nameof(SetUserProfileContextMessage);

        public static void AddSubscriber(string message, Action<object?> handler)
        {
            subscribers.Add((message, handler));
        }

        public static void Publish(string message, object? payload)
        {
            try
            {
                foreach (var subscriber in subscribers
                    .ToArray()
                    .Where(subscriber => subscriber.message.IsEqualTo(message)))
                {
                    subscriber.handler.Invoke(payload);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}