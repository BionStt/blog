using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Data.Contracts.Repositories
{
    public interface IBlogStoryTagRepository
    {
        Task<List<Guid>> GetIdsByStoryIdAsync(Guid storyId,
                                              CancellationToken cancel = default);

        Task<List<BlogStoryTag>> GetByStoryIdAsync(Guid storyId,
                                                   CancellationToken cancel = default);

        Task DeleteRangeAsync(List<BlogStoryTag> tags,
                              CancellationToken cancel = default);

        Task<BlogStoryTag> AddAsync(BlogStoryTag blogStoryTag,
                                    CancellationToken cancel = default);

        Task AddRangeAsync(List<BlogStoryTag> blogStoryTags,
                           CancellationToken cancel = default);

        Task DeleteRangeAsync(IEnumerable<BlogStoryTag> tags,
                              CancellationToken cancel = default);

        Task DeleteRangeAsync(IEnumerable<Guid> tagIds,
                              Guid storyId,
                              CancellationToken cancel = default);

        Task DeleteAsync(BlogStoryTag blogStoryTag,
                         CancellationToken cancel = default);
    }
}