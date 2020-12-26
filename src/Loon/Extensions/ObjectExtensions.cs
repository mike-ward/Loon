using System;
using System.Globalization;

namespace Loon.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToStringInvariant(this object obj) => Convert.ToString(obj, CultureInfo.InvariantCulture) ?? "{null}";
    }
}