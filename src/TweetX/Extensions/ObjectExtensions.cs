using System;
using System.Globalization;

namespace TweetX.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToStringInvariant(this object obj) => Convert.ToString(obj, CultureInfo.InvariantCulture) ?? "{null}";
    }
}