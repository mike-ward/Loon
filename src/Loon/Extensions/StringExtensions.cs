using System;
using System.Runtime.CompilerServices;

namespace Loon.Extensions
{
    internal static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this string? a, string? b)
        {
            return string.CompareOrdinal(a, b) == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEqualTo(this string? a, string? b)
        {
            return string.CompareOrdinal(a, b) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualToIgnoreCase(this string? a, string? b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEqualToIgnoreCase(this string? a, string? b)
        {
            return !string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPopulated(this string? str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string TruncateWithEllipsis(this string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length) + "…";
            }
            return source;
        }
    }
}