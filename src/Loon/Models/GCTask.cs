using System;
using System.Threading.Tasks;

#pragma warning disable S1215 // "GC.Collect" should not be called

namespace Loon.Models
{
    internal static class GCTask
    {
        public static ValueTask Execute(Timeline _)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return default;
        }
    }
}