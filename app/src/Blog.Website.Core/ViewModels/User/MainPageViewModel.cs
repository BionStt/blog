using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Core.Helpers;

namespace Blog.Website.Core.ViewModels.User
{
    public class MainPageViewModel : CommonUserContentViewModel
    {
        public List<ShortBlogStoryViewModel> Stories { get; set; }

        public MainPageViewModel(List<BlogStory> stories, List<Tag> tags, Int32 page, Int32 pageSize,
            Int32 totalBlogStoriesCount, String mainTitlePart, String pageTitlePart, String description,
            String keywords)
            : base(tags, page, pageSize, totalBlogStoriesCount, mainTitlePart, pageTitlePart, description, keywords)
        {
            SetStories(stories, tags);
        }
        
        public MainPageViewModel(List<BlogStory> stories, List<Tag> tags, Int32 page, Int32 pageSize,
            Int32 totalBlogStoriesCount, String mainTitlePart, String extendTitlePart, String pageTitlePart,
            String description, String keywords)
            : base(tags, page, pageSize, totalBlogStoriesCount, mainTitlePart, extendTitlePart, pageTitlePart,
                description, keywords)
        {
            SetStories(stories, tags);
        }

        private void SetStories(List<BlogStory> stories, List<Tag> tags)
        {
            Stories = stories.IsEmpty()
                ? new List<ShortBlogStoryViewModel>(0)
                : stories.Select(b =>
                {
                    var tagIds = b.BlogStoryTags.Select(st => st.TagId);
                    var tagsViewModels = tags.Where(t => tagIds.Contains(t.Id))
                        .Select(t => new TagViewModel(t))
                        .ToList();

                    return new ShortBlogStoryViewModel(b, tagsViewModels);
                }).ToList();
        }
    }
}