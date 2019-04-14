using System;
using System.Collections.Generic;
using Blog.Core.Converters;

namespace Blog.Core.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Alias { get; set; }

        public String SeoTitle { get; set; }
        public String SeoDescription { get; set; }
        public String SeoKeywords { get; set; }

        public Int32 Score { get; set; }
        public Boolean IsPublished { get; set; }

        public List<BlogStoryTag> BlogStoryTags { get; set; }

        public Tag()
        {
        }

        public Tag(String name,
                   String alias = null)
        {
            Name = name;
            Alias = String.IsNullOrEmpty(alias)
                ? StringToUrlStandard.Convert(name.ToLowerInvariant())
                : alias;
        }

        public void Update(Tag targetEntity)
        {
            Name = targetEntity.Name;
            Alias = targetEntity.Alias;
            SeoTitle = targetEntity.SeoTitle;
            SeoDescription = targetEntity.SeoDescription;
            SeoKeywords = targetEntity.SeoKeywords;
            Score = targetEntity.Score;
            IsPublished = targetEntity.IsPublished;
        }
    }
}