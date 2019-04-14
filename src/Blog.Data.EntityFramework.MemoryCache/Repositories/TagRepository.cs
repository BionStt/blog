using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Queries;
using Blog.Data.Contracts.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Data.EntityFramework.MemoryCache.Repositories
{
    public class TagRepository : ITagRepository
    {
        private const String TopTagsKey = "topTags";

        private readonly ITagRepository _tagRepository;
        private readonly IMemoryCache _memoryCache;

        public TagRepository(ITagRepository tagRepository,
                             IMemoryCache memoryCache)
        {
            _tagRepository = tagRepository;
            _memoryCache = memoryCache;
        }

        public Task<Page<Tag>> GetPageAsync(TagsQuery query,
                                            CancellationToken cancel = default)
        {
            return _tagRepository.GetPageAsync(query, cancel);
        }

        public Task<List<Tag>> GetAsync(TagsQuery query,
                                        CancellationToken cancel = default)
        {
            return _tagRepository.GetAsync(query, cancel);
        }

        public async Task<List<Tag>> GetTopPublishedAsync(CancellationToken cancel = default)
        {
            if(_memoryCache.TryGetValue(TopTagsKey, out List<Tag> result))
            {
                return result;
            }

            var topTags = await _tagRepository.GetTopPublishedAsync(cancel);
            _memoryCache.Set(TopTagsKey,
                             topTags,
                             new MemoryCacheEntryOptions
                             {
                                 AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                             });

            return topTags;
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllPublishedAsync(cancel);
        }

        public Task<Tag> GetAsync(Guid id,
                                  CancellationToken cancel = default)
        {
            return _tagRepository.GetAsync(id, cancel);
        }

        public Task<Tag> GetAsync(String alias,
                                  CancellationToken cancel = default)
        {
            return _tagRepository.GetAsync(alias, cancel);
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                                      CancellationToken cancel = default)
        {
            return _tagRepository.GetTagWithBlogStoryTagsAsync(alias, cancel);
        }

        public Task DeleteAsync(Tag tag,
                                CancellationToken cancel = default)
        {
            return _tagRepository.DeleteAsync(tag, cancel).ContinueWith(task => { _memoryCache.Remove(TopTagsKey); }, cancel);
        }

        public Task UpdateAsync(Tag tag,
                                CancellationToken cancel)
        {
            return _tagRepository.UpdateAsync(tag, cancel).ContinueWith(task => { _memoryCache.Remove(TopTagsKey); }, cancel);
        }

        public Task<Tag> AddAsync(Tag tag,
                                  CancellationToken cancel)
        {
            return _tagRepository.AddAsync(tag, cancel)
                                 .ContinueWith(task =>
                                               {
                                                   _memoryCache.Remove(TopTagsKey);
                                                   return task.Result;
                                               },
                                               cancel);
        }

        public Task<Guid> GetTagIdAsync(String alias,
                                        CancellationToken cancel)
        {
            return _tagRepository.GetTagIdAsync(alias, cancel);
        }
    }
}