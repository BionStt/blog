using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Data.Contracts.Repositories.Base;

namespace Blog.Data.Contracts.Repositories
{
    public interface IBlogStoryRepository : IRepository<BlogStory>, IRepositoryAsync<BlogStory>
    {
        Task<BlogStory> GetAsync(Int32 id,
                                 CancellationToken cancel = default);

        Task<List<BlogStory>> GetAllPublishedModifedDescAsync(CancellationToken cancel = default);

        Task<List<BlogStory>> GetPerPageAsync(Int32 skip,
                                              Int32 top,
                                              StorySort sortType,
                                              StoryFilter filter,
                                              CancellationToken cancel = default);

        Task<BlogStory> GetByAliasAsync(String alias,
                                        CancellationToken cancel = default);

        Task<BlogStory> GetByAliasWithAuthorAsync(String alias,
                                                  CancellationToken cancel = default);

        Task<BlogStory> GetWithBlogStoryTagsAsync(Int32 blogStoryId,
                                                  CancellationToken cancel = default);

        Task<BlogStory> GetWithBlogStoryTagsAsync(String alias,
                                                  CancellationToken cancel = default);

        Task<List<BlogStory>> WhereWithTagsAsync(Expression<Func<BlogStory, Boolean>> predicate,
                                                 CancellationToken cancel = default);

        Task<List<BlogStory>> WhereWithTagsPerPageAsync(Expression<Func<BlogStory, Boolean>> predicate,
                                                        Int32 skip,
                                                        Int32 top,
                                                        StorySort sort,
                                                        StoryFilter filter,
                                                        CancellationToken cancel = default);
    }
}