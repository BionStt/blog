using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Data.Contracts.Repositories.Base;

namespace Blog.Data.Contracts.Repositories
{
    public interface ITagRepository : IRepository<Tag>, IRepositoryAsync<Tag>
    {
        Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default);

        Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default);

        Task<Tag> GetAsync(Int32 id,
                           CancellationToken cancel = default);

        Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                               CancellationToken cancel = default);
    }
}