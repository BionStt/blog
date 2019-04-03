using System;
using Blog.Core.Queries;

namespace Blog.Website.Models.Requests.Author
{
    public class GetTagsRequest : PageParameters
    {
        public Int32 Page { get; set; } = 1;

        public TagsQuery ToQuery(Int32 pageSize)
        {
            return new TagsQuery(GetOffset(Page, pageSize), pageSize);
        }
    }
}