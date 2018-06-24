using System;
using Blog.Core.Enums;

namespace Blog.Core.Entities
{
    public class SitemapItem
    {
        public String Url { get; set; }
        public DateTime? Modified { get; set; }
        public ChangeFrequency? ChangeFrequency { get; set; }
        public Double? Priority { get; set; }
    }
}