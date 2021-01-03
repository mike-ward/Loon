using Loon.Services;

namespace Loon.Interfaces
{
    public interface IPubSubService
    {
        event PubSubEventHandler? PubSubRaised;

        void Publish(string message, object? payload);
    }
}