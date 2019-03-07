using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Core.Helpers;

namespace Blog.Website.Core.ViewModels.User
{
    public abstract class CommonUserContentViewModel : PageContextViewModel
    {
        public List<TagViewModel> Tags { get; set; }

        protected CommonUserContentViewModel(List<Tag> tags,
                                             Int32 page,
                                             Int32 pageSize,
                                             Int32 totalBlogStoriesCount)
            : base(page, pageSize, totalBlogStoriesCount)
        {
            SetTags(tags);
        }

        private void SetTags(List<Tag> tags)
        {
            Tags = tags.IsEmpty()
                ? new List<TagViewModel>(0)
                : tags.Select(x => new TagViewModel(x)).ToList();
        }
    }
}