using System;
using Blog.Core.Containers;
using Blog.Core.Queries;

namespace Blog.Website.Models.Requests.Reader
{
    public class GetStoriesRequest : PageParameters
    {
        public Int32 Page { get; set; }
        
        public BlogStoryQuery ToQuery(Int32 pageSize)
        {
            if(Page <= 0)
            {
                Page = 1;
            }
            
            return new BlogStoryQuery(GetOffset(Page, pageSize) , pageSize);
        }
    }
}