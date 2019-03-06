using System;

namespace Blog.Website.Models.Requests
{
    public class PageParameters
    {
        public Int32 Offset { get; set; }

        public Int32 Limit { get; set; }

        public String OrderBy { get; set; }

        protected static Int32 GetOffset(Int32 pageNumber, Int32 pageSize)
        {
            if (pageNumber <= 0)
            {
                return 0;
            }

            return (pageNumber - 1) * pageSize;
        }
    }
}