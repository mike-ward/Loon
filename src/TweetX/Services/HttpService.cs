using System.Net.Http;

namespace TweetX.Services
{
    internal static class HttpService
    {
        public static HttpClient Http { get; } = new HttpClient();
    }
}