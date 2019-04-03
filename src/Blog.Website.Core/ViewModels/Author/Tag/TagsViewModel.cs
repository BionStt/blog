using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Containers;

namespace Blog.Website.Core.ViewModels.Author.Tag
{
    public class TagsViewModel
    {
        public Int32 Page { get; set; }
        public Int32 TotalPageCount { get; set; }
        public List<TagShort> Tags { get; set; }

        public TagsViewModel(List<Blog.Core.Entities.Tag> tags,
                             Int32 totalCount,
                             Int32 page,
                             Int32 pageSize)
        {
            Page = page <= 0 ? 1 : page;
            TotalPageCount = (Int32) Math.Ceiling((Double) totalCount / pageSize);
            Tags = tags == null
                ? new List<TagShort>(0)
                : tags.Select(x => new TagShort(x))
                      .ToList();
        }

        public TagsViewModel(Page<Blog.Core.Entities.Tag> tagPage,
                             Int32 page)
            : this(tagPage.Items, tagPage.TotalCount, page, tagPage.PageSize)
        {
        }
    }
}