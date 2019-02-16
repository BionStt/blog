using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml;
using Blog.Core.Converters;
using Blog.Core.Entities;
using Blog.Extensions.Helpers;
using Blog.Website.Core.ViewModels.Author.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace Blog.Website.Core.ViewModels.Author.BlogStories
{
    public class EditBlogStoryViewModel
    {
        [Range(0, Int32.MaxValue)]
        public Int32 Id { get; set; }

        [Required, MinLength(4)]
        public String Title { get; set; }

        [Required]
        public String Description { get; set; }

        public String Keywords { get; set; }

        public String CreateDate { get; set; }

        public String ModifiedDate { get; set; }

        public Boolean IsPublished { get; set; }

        public String Alias { get; set; }

        public String Content { get; set; }

        [Url]
        public String StoryImageUrl { get; set; }

        [Url]
        public String StoryThumbUrl { get; set; }

        public String TagsSelected { get; set; }

        public String Tags { get; set; }

        public String Categories { get; set; }

        [Range(0, Int32.MaxValue)]
        public Int32? CategoryId { get; set; }

        public String AccessToken { get; set; }

        public String ShareLink { get; set; }

        public EditBlogStoryViewModel()
        {
            Tags = "[]";
        }

        public EditBlogStoryViewModel(BlogStory blogStory)
        {
            if (blogStory != null)
            {
                Id = blogStory.Id;
                Title = blogStory.Title;
                Alias = blogStory.Alias;
                Description = blogStory.Description;
                Keywords = blogStory.Keywords;
                Content = blogStory.Content;
                StoryImageUrl = blogStory.StoryImageUrl;
                StoryThumbUrl = blogStory.StoryThumbUrl;
                CreateDate = blogStory.CreateDate.ToString("dd.MM.yyyy HH:mm");
                ModifiedDate = blogStory.ModifiedDate.ToString("dd.MM.yyyy HH:mm");
                IsPublished = blogStory.IsPublished;
                CategoryId = blogStory.CategoryId;
                AccessToken = blogStory.AccessToken;
                Tags = "[]";

                if (blogStory.BlogStoryTags != null)
                {
                    TagsSelected = blogStory.BlogStoryTags.Select(x => x.TagId).JoinToString(",");
                }
            }
        }

        public EditBlogStoryViewModel(BlogStory blogStory, IEnumerable<Blog.Core.Entities.Tag> tags, IUrlHelper url) : this(blogStory)
        {
            Tags = tags == null
                       ? "[]"
                       : JsonConvert.SerializeObject(tags.Select(x => new TagShort(x)).ToList(), Formatting.None);

            ShareLink = GetShareLink(url);
        }

        public BlogStory ToDomain()
        {
            String slug = null;

            if (String.IsNullOrWhiteSpace(Alias))
            {
                slug = StringToUrlStandard.Convert(Title.Trim());
            }
            else
            {
                Alias = Alias.ToLowerInvariant();
            }

            return new BlogStory
                   {
                       Id = Id,
                       Title = Title.Trim(),
                       Alias = slug ?? Alias,
                       Description = Description.Trim(),
                       Keywords = Keywords,
                       Content = Content ?? String.Empty,
                       StoryImageUrl = StoryImageUrl?.Trim(),
                       StoryThumbUrl = StoryThumbUrl?.Trim(),
                       CreateDate = String.IsNullOrWhiteSpace(CreateDate) ? DateTime.Now : DateTime.Parse(CreateDate),
                       ModifiedDate = DateTime.Now,
                       IsPublished = IsPublished,
                       CategoryId = CategoryId,
                       AccessToken = AccessToken
                   };
        }

        public void SetImageUrlIfNotExist(String url, Int32 thumbMaxWidth)
        {
            if (String.IsNullOrWhiteSpace(StoryImageUrl))
            {
                StoryImageUrl = url;
            }

            if (String.IsNullOrWhiteSpace(StoryThumbUrl))
            {
                StoryThumbUrl = url;
            }

            ResizeThumbImage(thumbMaxWidth);
        }

        public String GetShareLink(IUrlHelper url)
        {
            if (String.IsNullOrWhiteSpace(AccessToken))
            {
                return url.Action(IsPublished ? "Story" : "Preview", "BlogStory", new {alias = Alias});
            }

            return IsPublished
                       ? url.Action("Story", "BlogStory", new {alias = Alias})
                       : url.Action("Preview", "BlogStory", new {alias = Alias, token = AccessToken});
        }

        private void ResizeThumbImage(Int32 thumbMaxWidth)
        {
            if (String.IsNullOrWhiteSpace(StoryThumbUrl))
            {
                return;
            }

            var uri = new Uri(StoryImageUrl);
            var query = QueryHelpers.ParseQuery(uri.Query);
            query.TryGetValue("width", out var widthValue);
            query.TryGetValue("height", out var heightValue);

            if (String.IsNullOrWhiteSpace(widthValue) || String.IsNullOrWhiteSpace(heightValue))
            {
                return;
            }

            var width = Double.Parse(widthValue);
            var height = Double.Parse(heightValue);
            var ratio = width / height;

            var thumbHeight = (Int32) Math.Round(thumbMaxWidth / ratio, MidpointRounding.ToEven);

            query["width"] = thumbMaxWidth.ToString();
            query["height"] = thumbHeight.ToString();

            var thumbUrl = $@"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}";
            StoryThumbUrl = QueryHelpers.AddQueryString(thumbUrl, query.ToDictionary(k => k.Key, v => v.Value.ToString()));
        }
    }
}