using System;
using System.Collections.Concurrent;
using Twitter.Models;

namespace Loon.Services
{
    public static class PubSubs
    {
        public static PubSubService OpenPreviousTab { get; } = new();
        public static PubSubService<TwitterStatus> AddStatus { get; } = new();
        public static PubSubService<TwitterStatus?> OpenWriteTab { get; } = new();
        public static PubSubService<object?> SetUserProfileContext { get; } = new(); // waiting for discriminated unions
    }

    public class PubSubService<T>
    {
        private readonly ConcurrentBag<Action<T>> subscribers = new();

        public void Subscribe(Action<T> handler)
        {
            subscribers.Add(handler);
        }

        public void Publish(T payload)
        {
            try
            {
                foreach (var subscriber in subscribers.ToArray())
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

    public class PubSubService
    {
        private readonly ConcurrentBag<Action> subscribers = new();

        public void Subscribe(Action handler)
        {
            subscribers.Add(handler);
        }

        public void Publish()
        {
            try
            {
                foreach (var subscriber in subscribers.ToArray())
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