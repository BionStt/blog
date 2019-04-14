using System;

namespace Blog.Website.Core.ViewModels.Author.Tag
{
    public class TagResume
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Int32 Score { get; set; }
        public String SeoTitle { get; set; }
        public Boolean IsPublished { get; set; }

        public TagResume()
        {
        }

        public TagResume(Blog.Core.Entities.Tag tag)
        {
            if (tag != null)
            {
                Id = tag.Id;
                Name = tag.Name;
                IsPublished = tag.IsPublished;
                Score = tag.Score;
                SeoTitle = tag.SeoTitle;
            }
        }
    }
}