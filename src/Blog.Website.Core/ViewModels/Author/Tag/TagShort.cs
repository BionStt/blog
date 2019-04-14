using System;
using Newtonsoft.Json;

namespace Blog.Website.Core.ViewModels.Author.Tag
{
    public class TagShort
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Boolean IsPublished { get; set; }

        public TagShort()
        {
        }

        public TagShort(Blog.Core.Entities.Tag tag)
        {
            if (tag != null)
            {
                Id = tag.Id;
                Name = tag.Name;
                IsPublished = tag.IsPublished;
            }
        }
    }
}