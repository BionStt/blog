using System;
using System.Collections.Generic;

namespace Blog.Core.Containers
{
    public class Page<T>
    {
        public List<T> Items { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 TotalCount { get; set; }
    }
}