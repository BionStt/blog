using System;

namespace Blog.Core.Queries
{
    public class BlogStoryQuery : BaseQuery
    {
        public Boolean? IsPublished { get; set; }

        public Guid? TagId { get; set; }
        public Guid[] StoriesIds { get; set; }

        public BlogStoryQuery(Int32 offset = DefaultOffset,
                              Int32 limit = DefaultLimit) : base(offset, limit)
        {
            DefaultOrder = ("create", false);
        }

        public static BlogStoryQuery AllPublished => new BlogStoryQuery(0, Int32.MaxValue)
        {
            IsPublished = true
        };
    }
}