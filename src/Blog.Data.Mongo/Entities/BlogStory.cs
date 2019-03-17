using System;
using System.Collections.Generic;
using Blog.Core.Enums;

namespace Blog.Data.Mongo.Entities
{
    public class BlogStory
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Alias { get; set; }
        public String Description { get; set; }
        public String Keywords { get; set; }
        public String Content { get; set; }
        public String StoryImageUrl { get; set; }
        public String StoryThumbUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public Boolean IsPublished { get; set; }
        public Int32 CommentsCount { get; set; }
        public Int32? CategoryId { get; set; }
        public Language Language { get; set; }
        public String AccessToken { get; set; }
        
        public List<Guid> TagIds { get; set; }
    }
}