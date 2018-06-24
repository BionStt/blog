using System;

namespace Blog.Website.Core.Requests
{
    public class CategoryBlogStoryRequest
    {
        public Int32 CategoryId { get; set; }
        public Int32 BlogStoryId { get; set; }
    }
}