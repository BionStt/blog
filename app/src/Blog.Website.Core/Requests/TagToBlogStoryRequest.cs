using System;

namespace Blog.Website.Core.Requests
{
    public class TagToBlogStoryRequest
    {
        public Int32 BlogStoryId { get; set; }
        public Int32 TagId { get; set; }
    }
}