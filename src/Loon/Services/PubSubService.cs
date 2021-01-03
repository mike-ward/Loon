using Loon.Interfaces;
using Loon.Models;

namespace Loon.Services
{
    public delegate void PubSubEventHandler(object? sender, PubSubEventArgs e);

    public class PubSubService : IPubSubService
    {
        public event PubSubEventHandler? PubSubRaised;

        public void Publish(string message, object? payload)
        {
            PubSubRaised?.Invoke(this, new PubSubEventArgs(message, payload));
        }
    }
}