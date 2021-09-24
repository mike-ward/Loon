using System;
using System.Collections.Concurrent;
using System.Threading;
using Twitter.Models;

namespace Loon.Services
{
    internal static class PubSubs
    {
        public static PubSubService                 OpenPreviousTab       { get; } = new();
        public static PubSubService                 UpdateLikesTimeline   { get; } = new();
        public static PubSubService<TwitterStatus>  AddStatus             { get; } = new();
        public static PubSubService<TwitterStatus?> OpenWriteTab          { get; } = new();
        public static PubSubService<object?>        SetUserProfileContext { get; } = new(); // waiting for discriminated unions
    }

    internal class PubSubService<T>
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
            subscribers.TryRemove(id, out var _);
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

    internal class PubSubService
    {
        private          int                               nextId;
        private readonly ConcurrentDictionary<int, Action> subscribers = new();

        public int Subscribe(Action handler)
        {
            var id = Interlocked.Increment(ref nextId);
            subscribers[id] = handler;
            return id;
        }

        public void Unsubscribe(int id)
        {
            subscribers.TryRemove(id, out var _);
        }

        public void Publish()
        {
            try
            {
                foreach (var subscriber in subscribers.Values)
                {
                    subscriber.Invoke();
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}