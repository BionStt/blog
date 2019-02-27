using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Queries;

namespace Blog.Data.Contracts.Repositories
{
    public interface IBlogStoryRepository
    {
        Task<Page<BlogStory>> GetAsync(StoriesQuery query,
                                       CancellationToken cancel = default);
        
        Task<BlogStory> GetAsync(Guid id,
                                 CancellationToken cancel = default);

        Task<BlogStory> GetAsync(String alias,
                                 CancellationToken cancel = default);

        Task<BlogStory> GetWithBlogStoryTagsAsync(Guid id,
                                                  CancellationToken cancel = default);

        Task<BlogStory> GetWithBlogStoryTagsAsync(String alias,
                                                  CancellationToken cancel = default);

        Task<List<BlogStory>> WhereWithTagsPerPageAsync(Expression<Func<BlogStory, Boolean>> predicate,
                                                        Int32 skip,
                                                        Int32 top,
                                                        StorySort sort,
                                                        StoryFilter filter,
                                                        CancellationToken cancel = default);
    }
}