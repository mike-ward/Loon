using System;
using System.Globalization;
using System.Linq;

namespace Loon.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToStringInvariant(this object obj)
        {
            return Convert.ToString(obj, CultureInfo.InvariantCulture) ?? "{null}";
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
            where T : class
            where TU : class
        {
            var sourceProps = typeof(T)
               .GetProperties()
               .Where(x => x.CanRead);

            var destProps = typeof(TU)
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