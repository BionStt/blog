using System;
using System.Collections.Generic;
using Blog.Core.Enums;

namespace Blog.Core.Entities
{
    public class BlogStory
    {
        public Guid Id { get; set; }
        public String Alias { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Content { get; set; }

        public String SeoDescription { get; set; }
        public String SeoKeywords { get; set; }

        public String StoryImageUrl { get; set; }
        public String StoryThumbUrl { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public Language Language { get; set; }

        public String AccessToken { get; set; }

        public List<BlogStoryTag> BlogStoryTags { get; set; }

        public void Update(BlogStory target)
        {
            Title = target.Title;
            Description = target.Description;
            SeoDescription = target.SeoDescription;
            SeoKeywords = target.SeoKeywords;
            Content = target.Content;
            StoryImageUrl = target.StoryImageUrl;
            StoryThumbUrl = target.StoryThumbUrl;
            Alias = target.Alias;
            AccessToken = target.AccessToken;
            Language = target.Language;
        }

        public void InitializeOnCreate()
        {
            AccessToken = Guid.NewGuid()
                              .ToString("N")
                              .Substring(0, 6);
        }
    }
}