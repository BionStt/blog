using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using Blog.Data.EntityFramework.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Repository
{
    public class BlogStoryRepository : BaseRepository<BlogStory>, IBlogStoryRepository
    {
        public BlogStoryRepository(BlogContext context) : base(context)
        {
        }

        public Task<List<BlogStory>> GetAllPublishedModifedDescAsync(CancellationToken cancel = default)
        {
            return Entities.Where(x => x.PublishedDate.HasValue)
                           .OrderByDescending(x => x.ModifiedDate)
                           .ToListAsync(cancel);
        }

        public Task<BlogStory> GetAsync(Guid id, CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(b => b.Id == id, cancel);
        }

        public Task<List<BlogStory>> GetPerPageAsync(Int32 skip,
                                                     Int32 top,
                                                     StorySort sortType,
                                                     StoryFilter filter,
                                                     CancellationToken cancel = default)
        {
            IQueryable<BlogStory> query = Entities.Include(x => x.BlogStoryTags);
            if (filter == StoryFilter.Published)
            {
                query = query.Where(x => x.PublishedDate.HasValue);
            }
            else if (filter == StoryFilter.UnPublished)
            {
                query = query.Where(x => !x.PublishedDate.HasValue);
            }

            query = sortType == StorySort.CreateDate
                        ? query.OrderByDescending(x => x.CreateDate)
                        : query.OrderByDescending(x => x.PublishedDate);

            return query.Skip(skip)
                        .Take(top)
                        .ToListAsync(cancel);
        }

        public Task<BlogStory> GetByAliasAsync(String alias, CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(b => b.Alias.Equals(alias), cancel);
        }

        public Task<BlogStory> GetByAliasWithAuthorAsync(String alias, CancellationToken cancel = default)
        {
            return Entities.FirstOrDefaultAsync(b => b.Alias.Equals(alias), cancel);
        }

        public Task<BlogStory> GetWithBlogStoryTagsAsync(Guid id, CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public Task<BlogStory> GetWithBlogStoryTagsAsync(String alias, CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .FirstOrDefaultAsync(x => x.Alias.Equals(alias), cancel);
        }

        public Task<List<BlogStory>> WhereWithTagsAsync(Expression<Func<BlogStory, Boolean>> predicate, CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .Where(predicate)
                           .ToListAsync(cancel);
        }

        public Task<List<BlogStory>> WhereWithTagsPerPageAsync(Expression<Func<BlogStory, Boolean>> predicate,
                                                               Int32 skip,
                                                               Int32 top,
                                                               StorySort sort,
                                                               StoryFilter filter,
                                                               CancellationToken cancel = default)
        {
            IQueryable<BlogStory> query = Entities.Include(x => x.BlogStoryTags)
                                                  .Where(predicate);
            if (filter == StoryFilter.Published)
            {
                query = query.Where(x => x.PublishedDate.HasValue);
            }
            else if (filter == StoryFilter.UnPublished)
            {
                query = query.Where(x => !x.PublishedDate.HasValue);
            }

            query = sort == StorySort.CreateDate
                        ? query.OrderByDescending(x => x.CreateDate)
                        : query.OrderByDescending(x => x.PublishedDate);

            return query.Skip(skip)
                        .Take(top)
                        .ToListAsync(cancel);
        }
    }
}