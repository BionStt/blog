using System;

namespace Blog.Core.Queries
{
    public class StoriesQuery : BaseQuery
    {
        public StoriesQuery(Int32 offset = DefaultOffset,
                            Int32 limit = DefaultLimit) : base(offset, limit)
        {
            DefaultOrder = ("createDate", false);
        }
    }
}