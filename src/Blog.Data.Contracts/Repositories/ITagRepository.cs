using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Queries;

namespace Blog.Data.Contracts.Repositories
{
    public interface ITagRepository
    {
        Task<Page<Tag>> GetPageAsync(TagsQuery query,
                                     CancellationToken cancel = default);

        Task<List<Tag>> GetAsync(TagsQuery query,
                                 CancellationToken cancel = default);

        Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default);

        Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default);

        Task<Tag> GetAsync(Guid id,
                           CancellationToken cancel = default);

        Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                               CancellationToken cancel = default);

        Task DeleteAsync(Tag tag,
                         CancellationToken cancel = default);

        Task UpdateAsync(Tag tag,
                         CancellationToken cancel);

        Task<Tag> AddAsync(Tag tag,
                           CancellationToken cancel);
    }
}