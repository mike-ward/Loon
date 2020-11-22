using System.Runtime.CompilerServices;

namespace TweetX.Extensions
{
    public static class StringExtensions
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
    }
}