using System;
using System.Globalization;

namespace TweetX.Extensions
{
    internal static class ObjectExtensions

    {
        public static string ToStringInvariant(this object obj) => Convert.ToString(obj, CultureInfo.InvariantCulture) ?? "{null}";
    }
}