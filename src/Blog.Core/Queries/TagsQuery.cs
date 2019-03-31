using System;

namespace Blog.Core.Queries
{
    public class TagsQuery : BaseQuery
    {
        public Boolean? WithScores { get; set; }
        public Boolean? IsPublished { get; set; }

        public TagsQuery() : this(0, MaxLimit)
        {
        }
        
        public TagsQuery(Int32 offset = DefaultOffset,
                         Int32 limit = DefaultLimit) : base(offset, limit)
        {
            DefaultOrder = ("score", false);
        }
    }
}