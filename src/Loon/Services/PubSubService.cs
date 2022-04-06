using System;
using System.Collections.Concurrent;
using System.Threading;
using Loon.Models;
using Twitter.Models;

namespace Loon.Services
{
    internal static class PubSubs
    {
        public static PubSubService<Unit>           OpenPreviousTab       { get; } = new();
        public static PubSubService<Unit>           UpdateLikesTimeline   { get; } = new();
        public static PubSubService<TwitterStatus>  AddStatus             { get; } = new();
        public static PubSubService<TwitterStatus?> OpenWriteTab          { get; } = new();
        public static PubSubService<object?>        SetUserProfileContext { get; } = new(); // waiting for discriminated unions
    }

    internal sealed class PubSubService<T>
    {
        private          int                                  nextId;
        private readonly ConcurrentDictionary<int, Action<T>> subscribers = new();

        public int Subscribe(Action<T> handler)
        {
            var id = Interlocked.Increment(ref nextId);
            subscribers[id] = handler;
            return id;
        }

        public void Unsubscribe(int id)
        {
            subscribers.TryRemove(id, out _);
        }

        public void Publish(T payload)
        {
            try
            {
                foreach (var subscriber in subscribers.Values)
                {
                    subscriber.Invoke(payload);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}