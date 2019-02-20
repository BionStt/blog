using System;

namespace Blog.Core.Entities
{
    public class BlogStoryTag
    {
        public Guid BlogStoryId { get; set; }
        public Guid TagId { get; set; }
        
        public BlogStory BlogStory { get; set; }
        public Tag Tag { get; set; }

        public BlogStoryTag() { }

        public BlogStoryTag(Guid blogStoryId, Guid tagId)
        {
            BlogStoryId = blogStoryId;
            TagId = tagId;
        }

        public BlogStoryTag(BlogStory blogStory, Tag tag)
        {
            BlogStory = blogStory;
            Tag = tag;
        }
    }
}