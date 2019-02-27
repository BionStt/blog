using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Data.Contracts.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default);

        Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default);

        Task<Tag> GetAsync(Guid id,
                           CancellationToken cancel = default);

        Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                               CancellationToken cancel = default);
    }
}