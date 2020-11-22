using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TweetX.Extensions;

namespace TweetX.Services
{
    internal static class TraceService
    {
        public static void Message(string msg, [CallerMemberName] string member = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            var message = $"TweetX: {msg.ToStringInvariant()} [{member.ToStringInvariant()}] {Path.GetFileName(path.ToStringInvariant())}({line.ToStringInvariant()})";
            Trace.WriteLine(message);
        }
    }
}