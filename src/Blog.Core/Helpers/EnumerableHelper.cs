using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Helpers
{
    public static class EnumerableHelper
    {
        public static Boolean IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || 
                   !collection.Any();
        }

        public static Boolean IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null &&
                   collection.Any();
        }
    }
}