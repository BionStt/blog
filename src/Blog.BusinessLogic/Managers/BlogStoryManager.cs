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

        public Task<BlogStory> GetWithTagsAsync(String alias,
                                                CancellationToken cancel = default)
        {
            return _blogStoryRepository.GetWithTagsAsync(alias, cancel);
        }

        public async Task<Tuple<Tag, List<BlogStory>>> GetTagStoriesByAliasAsync(String alias,
                                                                                 Int32 skip,
                                                                                 Int32 top,
                                                                                 StorySort sort,
                                                                                 StoryFilter filter,
                                                                                 CancellationToken cancel = default)
        {
            if(String.IsNullOrWhiteSpace(alias))
            {
                throw new EntityNotFoundException();
            }

            if(skip < 0)
            {
                skip = DefaultSkip;
            }

            if(top <= 0)
            {
                top = _defaultTop;
            }

            var tag = await _tagManager.GetTagWithBlogStoryTagsAsync(alias, cancel);
            if(tag == null)
            {
                throw new EntityNotFoundException();
            }

            var blogStoriesIds = tag.BlogStoryTags
                                    .Select(x => x.BlogStoryId)
                                    .ToArray();

            if(!blogStoriesIds.Any())
            {
                return new Tuple<Tag, List<BlogStory>>(null, new List<BlogStory>(0));
            }

            var stories = await _blogStoryRepository.GetAsync(new BlogStoryQuery(skip, top)
                                                              {
                                                                  StoriesIds = blogStoriesIds
                                                              },
                                                              cancel);

            return new Tuple<Tag, List<BlogStory>>(tag, stories);
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

        public async Task DeleteAsync(String alias,
                                      CancellationToken cancel = default)
        {
            if(String.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentException("Incorrect value for story alias");
            }

            var story = await _blogStoryRepository.GetAsync(alias, cancel);
            if(story == null)
            {
                throw new EntityNotFoundException($"Can't find story with alias : {alias}");
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