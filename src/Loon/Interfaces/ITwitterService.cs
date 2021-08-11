using Twitter.Services;

namespace Loon.Interfaces
{
    public interface ITwitterService
    {
        TwitterApi TwitterApi { get; }
    }
}