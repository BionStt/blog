using System;
using Blog.Core.Entities;

namespace Blog.Website.Core.ViewModels.User
{
    public class TagEditViewModel : TagViewModel
    {
        public Int32 Id { get; set; }
        public Int32 BlogStoryId { get; set; }

        public TagEditViewModel() { }
        public TagEditViewModel(Tag tag, Int32 blogStoryId) : base(tag)
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