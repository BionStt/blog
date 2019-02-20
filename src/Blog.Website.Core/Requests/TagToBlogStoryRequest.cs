using System;

namespace Blog.Website.Core.Requests
{
    public class TagToBlogStoryRequest
    {
        public Guid BlogStoryId { get; set; }
        public Guid TagId { get; set; }
    }
}