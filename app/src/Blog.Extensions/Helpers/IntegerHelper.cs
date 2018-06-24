using System;
using System.Collections.Generic;

namespace Blog.Extensions.Helpers
{
    public static class IntegerHelper
    {
        public static String JoinToString(this IEnumerable<Int32> numbers, String delimiter)
        {
            return numbers == null
                       ? String.Empty
                       : String.Join(delimiter, numbers);
        }
    }
}