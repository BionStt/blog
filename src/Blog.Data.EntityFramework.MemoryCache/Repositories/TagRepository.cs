using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Queries;
using Blog.Data.Contracts.Repositories;

namespace Blog.Data.EntityFramework.MemoryCache.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ITagRepository _tagRepositoryImplementation;

        public TagRepository(ITagRepository tagRepositoryImplementation)
        {
            _tagRepositoryImplementation = tagRepositoryImplementation;
        }

        public Task<Page<Tag>> GetPageAsync(TagsQuery query,
                                 CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetPageAsync(query, cancel);
        }

        public Task<List<Tag>> GetAsync(TagsQuery query,
                             CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetAsync(query, cancel);
        }

        public Task<List<Tag>> GetTopPublishedAsync(CancellationToken cancel = default)
        {
            
            
            return _tagRepositoryImplementation.GetTopPublishedAsync(cancel);
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetAllPublishedAsync(cancel);
        }

        public Task<Tag> GetAsync(Guid id,
                             CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetAsync(id, cancel);
        }

        public Task<Tag> GetAsync(String alias,
                             CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetAsync(alias, cancel);
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                                 CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.GetTagWithBlogStoryTagsAsync(alias, cancel);
        }

        public Task DeleteAsync(Tag tag,
                                CancellationToken cancel = default)
        {
            return _tagRepositoryImplementation.DeleteAsync(tag, cancel);
        }

        public Task UpdateAsync(Tag tag,
                                CancellationToken cancel)
        {
            return _tagRepositoryImplementation.UpdateAsync(tag, cancel);
        }

        public Task<Tag> AddAsync(Tag tag,
                             CancellationToken cancel)
        {
            return _tagRepositoryImplementation.AddAsync(tag, cancel);
        }

        public Task<Guid> GetTagIdAsync(String alias,
                                  CancellationToken cancel)
        {
            return _tagRepositoryImplementation.GetTagIdAsync(alias, cancel);
        }
    }
}