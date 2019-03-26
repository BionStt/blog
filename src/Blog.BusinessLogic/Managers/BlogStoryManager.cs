using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.BusinessLogic.Builders;
using Blog.BusinessLogic.Helpers;
using Blog.Core.Containers;
using Blog.Core.Contracts.Managers;
using Blog.Core.Entities;
using Blog.Core.Enums;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Exceptions;
using Blog.Core.Queries;
using Blog.Data.Contracts.Repositories;

namespace Blog.BusinessLogic.Managers
{
    public class BlogStoryManager : IBlogStoryManager
    {
        private readonly IBlogStoryRepository _blogStoryRepository;
        private readonly ITagManager _tagManager;

        public BlogStoryManager(IBlogStoryRepository blogStoryRepository,
                                ITagManager tagManager)
        {
            _blogStoryRepository = blogStoryRepository;
            _tagManager = tagManager;
        }

        public Task<Page<BlogStory>> GetPageAsync(BlogStoryQuery query,
                                                  CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetPageAsync(query, cancel);
        }

        public Task<Page<BlogStory>> GetPageWithTagsAsync(BlogStoryQuery query,
                                                          CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetPageWithTagsAsync(query, cancel);
        }

        public Task<BlogStory> GetAsync(String alias,
                                        CancellationToken cancel = default)
        {
            return String.IsNullOrWhiteSpace(alias)
                ? Task.FromResult<BlogStory>(null)
                : _blogStoryRepository.GetAsync(alias, cancel);
        }

        public Task<BlogStory> GetWithTagsAsync(Guid id,
                                                CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetWithTagsAsync(id, cancel);
        }

        public Task<BlogStory> GetPublishedWithTagsAsync(String alias,
                                                CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetPublishedWithTagsAsync(alias, cancel);
        }

        public async Task<BlogStory> CreateOrUpdateAsync(BlogStory blogStory,
                                                         CancellationToken cancel = default)
        {
            if(blogStory == null)
            {
                throw new ArgumentNullException(nameof(blogStory));
            }

            if(blogStory.Id == Guid.Empty)
            {
                blogStory.InitializeOnCreate();
                await _blogStoryRepository.AddAsync(blogStory, cancel);
                return blogStory;
            }

            var originalBlogStory = await _blogStoryRepository.GetAsync(blogStory.Id, cancel);
            if(originalBlogStory == null)
            {
                throw new EntityNotFoundException($"Can't edit story : {blogStory.Id}");
            }

            originalBlogStory.Update(blogStory);
            await _blogStoryRepository.UpdateAsync(originalBlogStory, cancel);
            return originalBlogStory;
        }

        public async Task<BlogStory> UpdateAccessTokenAsync(Guid id,
                                                            CancellationToken cancel = default)
        {
            var story = await _blogStoryRepository.GetAsync(id, cancel);
            if(story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {id}");
            }

            var accessToken = Guid.NewGuid()
                                  .ToString("N")
                                  .Substring(0, 6);

            story.AccessToken = accessToken;

            await _blogStoryRepository.UpdateAsync(story, cancel);
            return story;
        }

        public async Task<BlogStory> ChangeAvailabilityAsync(Guid id,
                                                             Boolean isPublished,
                                                             CancellationToken cancel = default)
        {
            var story = await _blogStoryRepository.GetAsync(id, cancel);
            if(story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {id}");
            }

            story.PublishedDate = isPublished
                ? DateTime.UtcNow
                : (DateTime?) null;

            await _blogStoryRepository.UpdateAsync(story, cancel);
            return story;
        }

        public async Task DeleteAsync(Guid storyId,
                                      CancellationToken cancel = default)
        {
            if(storyId == Guid.Empty)
            {
                throw new ArgumentException("Incorrect value for story id");
            }

            var story = await _blogStoryRepository.GetAsync(storyId, cancel);
            if(story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {storyId}");
            }

            await _blogStoryRepository.DeleteAsync(story, cancel);
        }

        public async Task<String> GetSiteMapXmlAsync(String baseUrl,
                                                     CancellationToken cancel = default)
        {
            var blogStories = await _blogStoryRepository.GetAsync(BlogStoryQuery.AllPublished, cancel);

            var siteMapBuilder = new SitemapBuilder();

            var modifiedDate = blogStories.Any()
                ? (DateTime?) blogStories.First().ModifiedDate
                : null;

            siteMapBuilder.AddUrl(baseUrl, modifiedDate, ChangeFrequency.Daily);

            var tags = await _tagManager.GetAllPublishedAsync(cancel);

            foreach (var tag in tags)
            {
                siteMapBuilder.AddUrl(tag.ToSitemapItem(baseUrl));
            }

            foreach (var blogStory in blogStories)
            {
                siteMapBuilder.AddUrl(blogStory.ToSitemapItem(baseUrl));
            }

            return siteMapBuilder.ToString();
        }

        public async Task RemoveAccessTokenAsync(Guid id,
                                                 CancellationToken cancel)
        {
            var story = await _blogStoryRepository.GetAsync(id, cancel);
            if(story == null)
            {
                throw new EntityNotFoundException($"Can't find story with id : {id}");
            }

            story.AccessToken = null;
            await _blogStoryRepository.UpdateAsync(story, cancel);
        }
    }
}