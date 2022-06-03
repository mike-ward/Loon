using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Loon.Extensions;

namespace Loon.Services
{
    internal static class TraceService
    {
        public static void Message(string msg, [CallerMemberName] string member = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            var message = $"Loon: {msg.ToStringInvariant()} [{member.ToStringInvariant()}] {Path.GetFileName(path.ToStringInvariant())}({line.ToString(CultureInfo.InvariantCulture)})";
            Trace.WriteLine(message);
        }
    }
}