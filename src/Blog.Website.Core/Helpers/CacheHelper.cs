using System;
using System.Collections.Generic;
using Blog.Core.Entities;
using Blog.Core.Enums.Sorting;
using Blog.Website.Core.Cache;
using Blog.Website.Core.ViewModels.User;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Website.Core.Helpers
{
    public static class CacheHelper
    {
        public static void SetSliding<T>(this IMemoryCache cache, Object key, T value, Int32 minutesCount)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(minutesCount));
            cache.Set(key, value, cacheEntryOptions);
        }

        public static BlogStory GetStory(this IMemoryCache cache, String alias)
        {
            return cache.Get<BlogStory>($"{CacheKeys.StoryPrefix}{alias}");
        }

        public static MainPageViewModel GetStoryPage(this IMemoryCache cache, Int32 page, Int32 count, StorySort sort)
        {
            return cache.Get<MainPageViewModel>($"{CacheKeys.StoriesPagePrefix}{page}-{count}-{sort}");
        }

        public static MainPageViewModel GetStoryCategoryPage(this IMemoryCache cache, String alias, Int32 page, Int32 count, StorySort sort)
        {
            return cache.Get<MainPageViewModel>($"{CacheKeys.CategoryStoriesPagePrefix}{alias}-{page}-{count}-{sort}");
        }

        public static MainPageViewModel GetStoryTagPage(this IMemoryCache cache, String alias, Int32 page, Int32 count, StorySort sort)
        {
            return cache.Get<MainPageViewModel>($"{CacheKeys.TagStoriesPagePrefix}{alias}-{page}-{count}-{sort}");
        }

        public static List<Tag> GetTags(this IMemoryCache cache)
        {
            return cache.Get<List<Tag>>(CacheKeys.Tags);
        }

        public static void SetSlidingStory(this IMemoryCache cache, String alias, BlogStory story, Int32 minutesCount)
        {
            var key = $"{CacheKeys.StoryPrefix}{alias}";
            cache.SetSliding(key, story, minutesCount);
            cache.RemovePages();
        }

        public static void SetStoryPageSliding(this IMemoryCache cache, MainPageViewModel viewModel, StorySort sort, Int32 minutesCount)
        {
            var key = $"{CacheKeys.StoriesPagePrefix}{viewModel.PageNumberNumber}-{viewModel.PageSize}-{sort}";
            var pages = cache.Get<List<String>>(CacheKeys.DefaultPagesStories);
            if (pages == null)
            {
                pages = new List<String> { key };
            }
            else if (!pages.Contains(key))
            {
                pages.Add(key);
            }

            cache.SetSliding(CacheKeys.DefaultPagesStories, pages, minutesCount);
            cache.SetSliding(key, viewModel, minutesCount);
        }

        public static void SetStoryCategoryPageSliding(this IMemoryCache cache, MainPageViewModel viewModel, String alias, StorySort sort, Int32 minutesCount)
        {
            var key = $"{CacheKeys.CategoryStoriesPagePrefix}{alias}-{viewModel.PageNumberNumber}-{viewModel.PageSize}-{sort}";
            var pages = cache.Get<List<String>>(CacheKeys.CategoryPagesStories);
            if (pages == null)
            {
                pages = new List<String> { key };
            }
            else if (!pages.Contains(key))
            {
                pages.Add(key);
            }

            cache.SetSliding(CacheKeys.CategoryPagesStories, pages, minutesCount);
            cache.SetSliding(key, viewModel, minutesCount);
        }

        public static void SetStoryTagPageSliding(this IMemoryCache cache, MainPageViewModel viewModel, String alias, StorySort sort, Int32 minutesCount)
        {
            var key = $"{CacheKeys.TagStoriesPagePrefix}{alias}-{viewModel.PageNumberNumber}-{viewModel.PageSize}-{sort}";
            var pages = cache.Get<List<String>>(CacheKeys.TagPagesStories);
            if (pages == null)
            {
                pages = new List<String> { key };
            }
            else if (!pages.Contains(key))
            {
                pages.Add(key);
            }

            cache.SetSliding(CacheKeys.TagPagesStories, pages, minutesCount);
            cache.SetSliding(key, viewModel, minutesCount);
        }

        public static void SetTags(this IMemoryCache cache, List<Tag> tags, Int32 minutesCount)
        {
            cache.SetSliding(CacheKeys.Tags, tags, minutesCount);
        }

        public static void RemoveStory(this IMemoryCache cache, String alias)
        {
            var key = $"{CacheKeys.StoryPrefix}{alias}";
            cache.Remove(key);
            cache.RemovePages();
        }

        public static void RemovePages(this IMemoryCache cache)
        {
            cache.RemovePages(CacheKeys.DefaultPagesStories);
            cache.RemovePages(CacheKeys.CategoryPagesStories);
            cache.RemovePages(CacheKeys.TagPagesStories);
            cache.RemoveTags();
        }

        public static void RemovePages(this IMemoryCache cache, String key)
        {
            var pages = cache.Get<List<String>>(key);
            if (pages != null)
            {
                foreach (var page in pages)
                {
                    cache.Remove(page);
                }

                cache.Remove(key);
            }
        }

        public static void RemoveTags(this IMemoryCache cache)
        {
            cache.Remove(CacheKeys.Tags);
        }

        public static void RemoveCategories(this IMemoryCache cache)
        {
            cache.Remove(CacheKeys.Categories);
        }
    }
}