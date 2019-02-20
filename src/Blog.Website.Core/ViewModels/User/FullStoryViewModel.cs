using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Core.Helpers;
using Blog.Extensions.Helpers;

namespace Blog.Website.Core.ViewModels.User
{
    public class FullStoryViewModel
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Keywords { get; set; }
        public String Content { get; set; }
        public String StoryThumbUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public String CreateDayDate { get; set; }
        public String CreateMonthYearDate { get; set; }
        public String Slug { get; set; }
        public Int32 AutorId { get; set; }
        public String StoryImageUrl { get; set; }
        public String StorySrcUrl { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean IsPublished { get; set; }
        public Int32 CommentsCount { get; set; }
        public List<TagViewModel> StoryTags { get; set; }
        public List<TagViewModel> MenuTags { get; set; }
        
        public FullStoryViewModel()
        {
        }

        public FullStoryViewModel(BlogStory story)
        {
            Id = story.Id;
            Title = story.Title;
            Keywords = story.Keywords;
            Description = story.Description;
            StoryThumbUrl = story.StoryThumbUrl;
            CreateDate = story.CreateDate;
            if (story.PublishedDate.HasValue)
            {
                CreateDayDate = story.PublishedDate.Value.Day.ToString();
                CreateMonthYearDate = $"{story.PublishedDate.Value.GetShorMonthName()} {story.PublishedDate.Value.Year}";
            }
            else
            {
                CreateDayDate = story.CreateDate.Day.ToString();
                CreateMonthYearDate = $"{story.CreateDate.GetShorMonthName()} {story.CreateDate.Year}";
            }
            
            Slug = story.Alias;
            Content = story.Content;
            StoryImageUrl = story.StoryImageUrl;
            ModifiedDate = story.ModifiedDate;
            IsPublished = story.IsPublished;
            CommentsCount = story.CommentsCount;

            StoryTags = story.BlogStoryTags.IsNotEmpty() 
                ? story.BlogStoryTags.Select(x=> new TagViewModel(x.Tag)).ToList() 
                : new List<TagViewModel>(0);
        }
        
        public FullStoryViewModel(BlogStory story, List<Tag> tags) : this(story)
        {
            MenuTags = tags.IsEmpty()
                ? new List<TagViewModel>(0)
                : tags.Select(x => new TagViewModel(x)).ToList();
        }
    }
}