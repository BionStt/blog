using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.BusinessLogic.Builders;
using Blog.BusinessLogic.Helpers;
using Blog.Core.Contracts.Managers;
using Blog.Core.Entities;
using Blog.Core.Enums;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Exceptions;
using Blog.Data.Contracts.Repositories;

namespace Blog.BusinessLogic.Managers
{
    public class BlogStoryManager : IBlogStoryManager
    {
        private const Int32 DefaultSkip = 0;
        private readonly Int32 _defaultTop;

        private readonly IBlogStoryRepository _blogStoryRepository;
        private readonly ITagManager _tagManager;

        public BlogStoryManager(IBlogStoryRepository blogStoryRepository,
                                ITagManager tagManager,
                                Int32 defaultTop)
        {
            _blogStoryRepository = blogStoryRepository;
            _tagManager = tagManager;
            _defaultTop = defaultTop;
        }

        public Task<List<BlogStory>> GetAllAsync(CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetAllAsync(cancel);
        }

        public Task<BlogStory> GetAsync(String alias,
                                        CancellationToken cancel = default)
        {
            return String.IsNullOrWhiteSpace(alias)
                       ? null
                       : _blogStoryRepository.GetByAliasAsync(alias, cancel);
        }

        public async Task<BlogStory> GetWithTagsAsync(Int32 id, CancellationToken cancel = default)
        {
            if (id <= 0)
            {
                return null;
            }

            var story = await _blogStoryRepository.GetWithBlogStoryTagsAsync(id, cancel);
            if (story != null)
            {
                var tagIds = story.BlogStoryTags.Select(x => x.TagId);
                var tags = await _tagManager.GetAsync(tagIds, cancel);
                story.BlogStoryTags.ForEach(tag => tag.Tag = tags.FirstOrDefault(x => x.Id == tag.TagId));
            }

            return story;
        }

        public async Task<BlogStory> GetWithTagsAsync(String alias,
                                                      CancellationToken cancel = default)
        {
            var story = await _blogStoryRepository.GetWithBlogStoryTagsAsync(alias, cancel);
            if (story != null)
            {
                var tagIds = story.BlogStoryTags.Select(x => x.TagId);
                var tags = await _tagManager.GetAsync(tagIds, cancel);
                story.BlogStoryTags.ForEach(tag => tag.Tag = tags.FirstOrDefault(x => x.Id == tag.TagId));
            }

            return story;
        }

        public Task<List<BlogStory>> GetAsync(Int32 skip,
                                              Int32 top,
                                              StorySort sort,
                                              StoryFilter filter,
                                              CancellationToken cancel = default)
        {
            if (skip < 0)
            {
                skip = DefaultSkip;
            }

            if (top <= 0)
            {
                top = _defaultTop;
            }

            return _blogStoryRepository.GetPerPageAsync(skip, top, sort, filter, cancel);
        }

        public async Task<Tuple<Tag, List<BlogStory>>> GetTagStoriesByAliasAsync(String alias,
                                                                                 Int32 skip,
                                                                                 Int32 top,
                                                                                 StorySort sort,
                                                                                 StoryFilter filter,
                                                                                 CancellationToken cancel = default)
        {
            if (String.IsNullOrWhiteSpace(alias))
            {
                throw new EntityNotFoundException();
            }

            if (skip < 0)
            {
                skip = DefaultSkip;
            }

            if (top <= 0)
            {
                top = _defaultTop;
            }

            var tag = await _tagManager.GetTagWithBlogStoryTagsAsync(alias, cancel);
            if (tag == null)
            {
                throw new EntityNotFoundException();
            }

            var blogStoriesIds = tag.BlogStoryTags.OrderByDescending(x => x.BlogStoryId)
                                    .Select(x => x.BlogStoryId)
                                    .Skip(skip)
                                    .Take(top)
                                    .ToList();

            if (!blogStoriesIds.Any())
            {
                return new Tuple<Tag, List<BlogStory>>(null, new List<BlogStory>(0));
            }

            var blogStories = await _blogStoryRepository.WhereWithTagsAsync(x => blogStoriesIds.Contains(x.Id), cancel);
            return new Tuple<Tag, List<BlogStory>>(tag, blogStories);
        }

        public async Task<BlogStory> CreateOrUpdateAsync(BlogStory blogStory,
                                                         CancellationToken cancel = default)
        {
            if (blogStory == null)
            {
                throw new ArgumentNullException(nameof(blogStory));
            }

            if (blogStory.Id <= 0)
            {
                await _blogStoryRepository.AddAsync(blogStory, cancel);
                return blogStory;
            }

            var originalBlogStory = await _blogStoryRepository.GetAsync(blogStory.Id, cancel);
            if (originalBlogStory == null)
            {
                throw new EntityNotFoundException($"Can't edit story : {blogStory.Id}");
            }

            originalBlogStory.Update(blogStory);
            await _blogStoryRepository.UpdateAsync(originalBlogStory, cancel);
            return originalBlogStory;
        }

        public async Task<BlogStory> CreateAccessTokenAsync(Int32 id,
                                                            CancellationToken cancel = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Incorrect value for story id");
            }

            var story = await _blogStoryRepository.GetAsync(id, cancel);
            if (story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {id}");
            }

            if (story.IsPublished || 
                !String.IsNullOrWhiteSpace(story.AccessToken))
            {
                return story;
            }

            var accessToken = Guid.NewGuid()
                                  .ToString("N")
                                  .Substring(0, 6);
            
            story.AccessToken = accessToken;
            
            await _blogStoryRepository.UpdateAsync(story, cancel);
            return story;
        }

        public async Task<BlogStory> ChangeAvailabilityAsync(Int32 id, Boolean isPublished,
                                                             CancellationToken cancel = default)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Incorrect value for story id");
            }

            var story = await _blogStoryRepository.GetAsync(id, cancel);
            if (story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {id}");
            }

            if (isPublished && !story.PublishedDate.HasValue)
            {
                story.PublishedDate = DateTime.UtcNow;
            }

            story.IsPublished = isPublished;

            await _blogStoryRepository.UpdateAsync(story, cancel);
            return story;
        }

        public async Task DeleteAsync(String alias, CancellationToken cancel = default)
        {
            if (String.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("Incorrect value for story alias");
            }

            var story = await _blogStoryRepository.GetByAliasAsync(alias, cancel);
            if (story == null)
            {
                throw new EntityNotFoundException($"Can't find story with alias : {alias}");
            }

            await _blogStoryRepository.DeleteAsync(story, cancel);
        }

        public async Task<String> GetSiteMapXmlAsync(String baseUrl,
                                                     CancellationToken cancel = default)
        {
            var blogStories = await _blogStoryRepository.GetAllPublishedModifedDescAsync(cancel);

            var siteMapBuilder = new SitemapBuilder();
            
            var modifiedDate = blogStories.Any() 
                                   ? (DateTime?) blogStories.First().ModifiedDate 
                                   : null;
            
            siteMapBuilder.AddUrl(baseUrl, modifiedDate, ChangeFrequency.Daily);

            foreach (var blogStory in blogStories)
            {
                siteMapBuilder.AddUrl(blogStory.ToSitemapItem(baseUrl));
            }

            return siteMapBuilder.ToString();
        }


        public Task<Int32> CountPublishedAsync(CancellationToken cancel = default)
        {
            return _blogStoryRepository.CountAsync(x => x.IsPublished, cancel);
        }

        public Task<Int32> CountStoriesForTagAsync(Int32 tagId, CancellationToken cancel = default)
        {
            return _tagManager.GetStoriesCountAsync(tagId, cancel);
        }

        public Task<Int32> CountAsync(CancellationToken cancel = default)
        {
            return _blogStoryRepository.CountAsync(cancel);
        }
    }
}