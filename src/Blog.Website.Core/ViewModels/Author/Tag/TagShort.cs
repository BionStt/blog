using System;
using Newtonsoft.Json;

namespace Blog.Website.Core.ViewModels.Author.Tag
{
    public class TagShort
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "isPublished")]
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