using System;
using Blog.Core.Entities;

namespace Blog.Website.Core.ViewModels.User
{
    public class TagEditViewModel : TagViewModel
    {
        public Guid Id { get; set; }
        public Guid BlogStoryId { get; set; }

        public TagEditViewModel() { }
        public TagEditViewModel(Tag tag, Guid blogStoryId) : base(tag)
        {
            BlogStoryId = blogStoryId;
        }

        public Tag ToDomain()
        {
            return new Tag
            {
                Id = Id,
                Name = Name
            };
        }
    }
}