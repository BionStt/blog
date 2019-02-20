using System;
using System.Collections.Generic;
using Blog.Core.Contracts.Entities;
using Blog.Core.Converters;

namespace Blog.Core.Entities
{
    public class Tag : IEntityUpdate<Tag>
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

        public Tag() { }

        public Tag(String name)
        {
            Name = name;
            Alias = StringToUrlStandard.Convert(name.ToLowerInvariant());
        }

        public void Update(Tag targetEntity)
        {
            Name = targetEntity.Name;
            SeoTitle = targetEntity.SeoTitle;
            SeoDescription = targetEntity.SeoDescription;
            SeoKeywords = targetEntity.SeoKeywords;
        }
    }
}