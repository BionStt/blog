using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Queries;

namespace Blog.Data.Contracts.Repositories
{
    public interface IBlogStoryRepository
    {
        Task<Page<BlogStory>> GetPageAsync(BlogStoryQuery query,
                                           CancellationToken cancel = default);

        Task<Page<BlogStory>> GetPageWithTagsAsync(BlogStoryQuery query,
                                                   CancellationToken cancel = default);

        Task<List<BlogStory>> GetAsync(BlogStoryQuery query,
                                       CancellationToken cancel = default);

        Task<BlogStory> GetAsync(Guid id,
                                 CancellationToken cancel = default);

        Task<BlogStory> GetAsync(String alias,
                                 CancellationToken cancel = default);

        Task<BlogStory> GetWithTagsAsync(Guid id,
                                         CancellationToken cancel = default);

        Task<BlogStory> GetPublishedWithTagsAsync(String alias,
                                         CancellationToken cancel = default);

        Task AddAsync(BlogStory blogStory,
                      CancellationToken cancel);

        Task UpdateAsync(BlogStory story,
                         CancellationToken cancel);

        Task DeleteAsync(BlogStory story,
                         CancellationToken cancel);
    }
}