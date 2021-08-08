using System;
using System.Threading.Tasks;

#pragma warning disable S1215 // "GC.Collect" should not be called

namespace Loon.Models
{
    internal static class CollectTask
    {
        public static ValueTask Execute(Timeline _)
        {
            GC.Collect();
            return default;
        }
    }
}