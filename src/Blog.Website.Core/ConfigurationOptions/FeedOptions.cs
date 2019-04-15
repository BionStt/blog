using System;

namespace Blog.Website.Core.ConfigurationOptions
{
    public class FeedOptions
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String GeneratorDescription { get; set; }
        public String SourceCodeLink { get; set; }
        public String Email { get; set; }
        public String AuthorName { get; set; }
        public String FullEmail => $"{Email} ({AuthorName})";
    }
}