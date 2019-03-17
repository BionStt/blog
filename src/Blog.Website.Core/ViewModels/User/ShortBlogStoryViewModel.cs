using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Extensions.Helpers;

namespace Blog.Website.Core.ViewModels.User
{
    public class ShortBlogStoryViewModel
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String ShortContent { get; }
        public String StoryThumbUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public String Alias { get; set; }
        public String CreateDayDate { get; set; }
        public String CreateMonthYearDate { get; set; }
        public List<TagViewModel> Tags { get; set; }
        
        
        public ShortBlogStoryViewModel() { }

        public ShortBlogStoryViewModel(BlogStory blogStory)
        {
            Id = blogStory.Id;
            Title = blogStory.Title;
            ShortContent = blogStory.Description;
            StoryThumbUrl = blogStory.StoryThumbUrl;
            Alias = blogStory.Alias;
            if (blogStory.PublishedDate.HasValue)
            {
                CreateDayDate = blogStory.PublishedDate.Value.Day.ToString();
                CreateMonthYearDate = $"{blogStory.PublishedDate.Value.GetShorMonthName()} {blogStory.PublishedDate.Value.Year.ToString()}";
            }
            else
            {
                CreateDayDate = blogStory.CreateDate.Day.ToString();
                CreateMonthYearDate = $"{blogStory.CreateDate.GetShorMonthName()} {blogStory.CreateDate.Year.ToString()}";
            }
            
            Tags = new List<TagViewModel>(0);
        }

        public ShortBlogStoryViewModel(BlogStory blogStory, List<TagViewModel> tags) : this(blogStory)
        {
            Tags = tags;
        }
    }
}