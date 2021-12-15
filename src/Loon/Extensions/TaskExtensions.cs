using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Loon.Services;

#pragma warning disable S3168        // "async" methods should not return "void"
#pragma warning disable AsyncFixer03 // Fire & forget async void methods

namespace Loon.Extensions
{
    internal static class TaskExtensions
    {
        [SuppressMessage("Usage", "VSTHRD003", MessageId = "Avoid awaiting foreign Tasks")]
        public static async Task FireAndForget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TraceService.Message(e.Message);
            }
        }

        public static async Task FireAndForget(this ValueTask task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TraceService.Message(e.Message);
            }
        }
    }
}