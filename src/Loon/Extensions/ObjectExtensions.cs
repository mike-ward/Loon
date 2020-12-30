using System;
using System.Globalization;
using System.Linq;

namespace Loon.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToStringInvariant(this object obj) => Convert.ToString(obj, CultureInfo.InvariantCulture) ?? "{null}";

        public static void CopyPropertiesTo<T, U>(this T source, U dest)
        {
            var sourceProps = typeof(T)
                .GetProperties()
                .Where(x => x.CanRead);

            var destProps = typeof(U)
                .GetProperties()
                .Where(x => x.CanWrite)
                .ToArray();

            foreach (var sourceProp in sourceProps)
            {
                Array.Find(destProps, x => x.Name.IsEqualTo(sourceProp.Name))
                    ?.SetValue(dest, sourceProp.GetValue(source, null), null);
            }
        }
    }
}