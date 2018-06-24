using System;
using System.Collections.Generic;

namespace Blog.Extensions.Helpers
{
    public static class DateTimeHelper
    {
        private static Dictionary<Int32,String> _shortMonthName = new Dictionary<Int32, String>
        {
            {1,"янв"},
            {2,"фев"},
            {3,"мар"},
            {4,"апр"},
            {5,"май"},
            {6,"июн"},
            {7,"июл"},
            {8,"авг"},
            {9,"сен"},
            {10,"окт"},
            {11,"ноя"},
            {12,"дек"}
        };


        public static String GetShorMonthName(this DateTime date)
        {
            var monthNumber = date.Month;
            return _shortMonthName[monthNumber];
        }
    }
}