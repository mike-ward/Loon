using System.Threading;

namespace Loon.Interfaces
{
    public interface ICancellationTokeSourceProvider
    {
        CancellationTokenSource CancellationTokenSource { get; }
    }
}