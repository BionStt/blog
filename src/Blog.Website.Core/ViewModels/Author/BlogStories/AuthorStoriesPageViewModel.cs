using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;

namespace Blog.Website.Core.ViewModels.Author.BlogStories
{
    public class AuthorStoriesPageViewModel
    {
        public Int32 Page { get; set; }
        public Int32 TotalPageCount { get; set; }
        public List<AuthorShortBlogStoryViewModel> Stories { get; set; }

        public AuthorStoriesPageViewModel(List<BlogStory> stories, Int32 page, Int32 totalBlogStoriesCount, Int32 pageSize)
        {
            var additionalPageCount = totalBlogStoriesCount % pageSize > 0 ? 1 : 0;
            TotalPageCount = totalBlogStoriesCount / pageSize + additionalPageCount;
            Page = page;

            if (stories == null || !stories.Any())
            {
                Stories = new List<AuthorShortBlogStoryViewModel>(0);
            }
            else
            {
                Stories = stories.Select(story => new AuthorShortBlogStoryViewModel(story)).ToList();
            }
        }
    }
}