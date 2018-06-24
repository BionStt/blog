using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Entities;
using Blog.Core.Enums;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;

namespace Blog.BusinessLogic.Helpers
{
    public static class BlogStoryExtensions
    {
        public static List<BlogStory> GetPerPage(this List<BlogStory> stories,
                                                 Int32 pageNumber,
                                                 Int32 perPage,
                                                 StorySort sortType,
                                                 StoryFilter filter)
        {
            IEnumerable<BlogStory> query = stories;

            if (filter == StoryFilter.Published)
            {
                query = query.Where(x => x.IsPublished);
            }
            else if (filter == StoryFilter.UnPublished)
            {
                query = query.Where(x => !x.IsPublished);
            }

            query = sortType == StorySort.CreateDate
                        ? query.OrderByDescending(x => x.CreateDate)
                        : query.OrderByDescending(x => x.PublishedDate);

            return query.Skip((pageNumber - 1) * perPage)
                        .Take(perPage).ToList();
        }

        public static SitemapItem ToSitemapItem(this BlogStory story, String baseUrl)
        {
            return new SitemapItem
                   {
                       Url = $"{baseUrl}/{story.Alias}",
                       Modified = story.ModifiedDate,
                       ChangeFrequency = ChangeFrequency.Daily,
                   };
        }
    }
}