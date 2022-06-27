using System;
using System.Net;
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

        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotNullOrWhiteSpace(this string? str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string TruncateWithEllipsis(this string source, int length)
        {
            return source.Length > length
                ? string.Concat(source.AsSpan(0, length), "…")
                : source;
        }

        public static string HtmlDecode(this string text)
        {
            // Twice to handle sequences like: "&amp;mdash;"
            return WebUtility.HtmlDecode(
                WebUtility.HtmlDecode(text));
        }
    }
}