using System;
using Blog.Core.Queries;

namespace Blog.Website.Models.Requests.Author
{
    public class GetStoriesRequest : PageParameters
    {
        public Int32 Page { get; set; } = 1;
        public String State { get; set; }

        public BlogStoryQuery ToQuery(Int32 pageSize)
        {
            return new BlogStoryQuery(GetOffset(Page, pageSize) , pageSize);
        }
    }
}