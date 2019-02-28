using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Data.Contracts.Repositories
{
    public interface IBlogStoryTagRepository
    {
        Task<List<BlogStoryTag>> GetByStoryIdAsync(Guid storyId,
                                                   CancellationToken cancel);

        Task DeleteRangeAsync(List<BlogStoryTag> tags,
                              CancellationToken cancel);

        Task<BlogStoryTag> AddAsync(BlogStoryTag blogStoryTag,
                                    CancellationToken cancel);

        Task AddRangeAsync(List<BlogStoryTag> blogStoryTags,
                           CancellationToken cancel);

        Task DeleteRangeAsync(IEnumerable<BlogStoryTag> tags,
                              CancellationToken cancel);

        Task DeleteAsync(BlogStoryTag blogStoryTag,
                         CancellationToken cancel);
    }
}