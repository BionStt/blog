using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Core.Helpers;

namespace Blog.Website.Core.ViewModels.User
{
    public class MainPageViewModel : PageContextViewModel
    {
        public List<ShortBlogStoryViewModel> Stories { get; set; }
        public List<TagViewModel> Tags { get; set; }

        public MainPageViewModel(List<BlogStory> stories,
                                 List<Tag> tags,
                                 Int32 page,
                                 Int32 pageSize,
                                 Int32 totalBlogStoriesCount)
            : base(page, pageSize, totalBlogStoriesCount)
        {
            SetStories(stories, tags);
            SetTags(tags);
        }

        private void SetStories(List<BlogStory> stories,
                                List<Tag> tags)
        {
            Stories = stories.IsEmpty()
                ? new List<ShortBlogStoryViewModel>(0)
                : stories.Select(b =>
                          {
                              if(b.BlogStoryTags.IsEmpty())
                              {
                                  return new ShortBlogStoryViewModel(b);
                              }

                              var tagsViewModels = b.BlogStoryTags
                                                    .Select(t => new TagViewModel(t.Tag))
                                                    .ToList();

                              return new ShortBlogStoryViewModel(b, tagsViewModels);
                          })
                         .ToList();
        }

        private void SetTags(List<Tag> tags)
        {
            Tags = tags.IsEmpty()
                ? new List<TagViewModel>(0)
                : tags.Select(x => new TagViewModel(x))
                      .ToList();
        }
    }
}