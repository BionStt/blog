using System;

namespace Blog.Core.Queries
{
    public class StoriesQuery : BaseQuery
    {
        public Boolean? IsPublished { get; set; }

        public String[] Tags { get; set; }
        public Guid[] StoriesIds { get; set; }

        public StoriesQuery(Int32 offset = DefaultOffset,
                            Int32 limit = DefaultLimit) : base(offset, limit)
        {
            DefaultOrder = ("create", false);
        }

        public static StoriesQuery AllPublished => new StoriesQuery(0, Int32.MaxValue)
        {
            IsPublished = true
        };
    }
}