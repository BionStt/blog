using System;
using Blog.Core.Queries;

namespace Blog.Website.Models.Requests.Reader
{
    public class GetStoriesRequest : PageParameters
    {
        public Int32 Page { get; set; }
        
        public StoriesQuery ToQuery(Int32 pageSize)
        {
            return new StoriesQuery(GetOffset(Page, pageSize) , pageSize);
        }
    }
}