using System;

namespace Loon.Models
{
    public class PubSubEventArgs : EventArgs
    {
        public string Message { get; }
        public object? Payload { get; }

        public PubSubEventArgs(string message, object? payload)
        {
            Message = message;
            Payload = payload;
        }
    }
}