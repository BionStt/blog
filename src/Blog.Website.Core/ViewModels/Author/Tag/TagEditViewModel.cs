using System;

namespace Blog.Website.Core.ViewModels.Author.Tag
{
    public class TagEditViewModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Alias { get; set; }
        public String SeoTitle { get; set; }
        public String SeoDescription { get; set; }
        public String Keywords { get; set; }
        public Int32 Score { get; set; }

        public TagEditViewModel()
        {
        }

        public TagEditViewModel(Blog.Core.Entities.Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            Alias = tag.Alias;
            SeoTitle = tag.SeoTitle;
            SeoDescription = tag.SeoDescription;
            Keywords = tag.SeoKeywords;
            Score = tag.Score;
        }

        public Blog.Core.Entities.Tag ToDomain()
        {
            return new Blog.Core.Entities.Tag
            {
                Id = Id,
                Name = Name,
                Alias = Alias,
                SeoTitle = SeoTitle,
                SeoDescription = SeoDescription,
                SeoKeywords = Keywords,
                Score = Score
            };
        }
    }
}