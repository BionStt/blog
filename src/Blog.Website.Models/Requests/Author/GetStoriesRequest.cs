using System;
using Blog.Core.Queries;

namespace Blog.Website.Models.Requests.Author
{
    public class GetStoriesRequest : PageParameters
    {
        public Int32 Page { get; set; }
        public String State { get; set; }
        public String OrderBy { get; set; }

        public StoriesQuery ToQuery(Int32 pageSize)
        {
            return new StoriesQuery(GetOffset(Page, pageSize) , pageSize);
        }
    }
}