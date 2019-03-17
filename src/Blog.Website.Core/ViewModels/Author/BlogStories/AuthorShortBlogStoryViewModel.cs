using System;
using Blog.Core.Entities;
using Blog.Website.Core.Enums;

namespace Blog.Website.Core.ViewModels.Author.BlogStories
{
    public class AuthorShortBlogStoryViewModel
    {
        public Guid Id { get; set; }
        public String Alias { get; set; }
        public String Title { get; set; }
        public String CreatedDate { get; set; }
        public String PublisheddDate { get; set; }
        public BlosStoryStatus Status { get; set; }

        public AuthorShortBlogStoryViewModel(BlogStory story)
        {
            Id = story.Id;
            Title = story.Title;
            Alias = story.Alias;
            CreatedDate = story.CreateDate.ToString("dd.MM.yyyy HH:mm");
            if (story.PublishedDate.HasValue)
            {
                PublisheddDate = story.PublishedDate.Value.ToString("dd.MM.yyyy HH:mm");
            }

            Status = story.PublishedDate.HasValue ? BlosStoryStatus.Published : BlosStoryStatus.Draft;
        }
    }
}