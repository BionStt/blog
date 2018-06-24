using System;

namespace Blog.Core.Entities
{
    public class BlogStoryTag
    {
        public Int32 BlogStoryId { get; set; }
        public Int32 TagId { get; set; }
        
        public BlogStory BlogStory { get; set; }
        public Tag Tag { get; set; }

        public BlogStoryTag() { }

        public BlogStoryTag(Int32 blogStoryId, Int32 tagId)
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