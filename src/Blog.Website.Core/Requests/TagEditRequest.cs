using System;
using System.Globalization;
using Blog.Core.Entities;

namespace Blog.Website.Core.Requests
{
    public class TagEditRequest
    {
        public Guid Id { get; }
        public String Name { get; set; }
        public String Alias { get; set; }
        public String SeoTitle { get; set; }
        public String SeoDescription { get; set; }
        public String SeoKeywords { get; set; }

        public Int32 Score { get; set; }
        public Boolean IsPublished { get; set; }

        public TagEditRequest(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            Alias = tag.Alias;
            SeoTitle = tag.SeoTitle;
            SeoDescription = tag.SeoDescription;
            SeoKeywords = tag.SeoKeywords;
            Score = tag.Score;
            IsPublished = tag.IsPublished;
        }

        public Tag ToDomain()
        {
            return new Tag
            {
                Id =  Id,
                Name = Name,
                Alias = Alias,
                SeoTitle = SeoTitle,
                SeoDescription = SeoDescription,
                SeoKeywords = SeoKeywords,
                Score = Score,
                IsPublished = IsPublished
            };
        }
    }
}