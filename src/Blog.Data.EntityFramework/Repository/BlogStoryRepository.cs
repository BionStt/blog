using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Helpers;
using Blog.Core.Queries;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using Blog.Data.EntityFramework.Helpers;
using Blog.Data.EntityFramework.OrderMappings;
using GenRep.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Repository
{
    public class BlogStoryRepository : BaseRepository<BlogStory>,
                                       IBlogStoryRepository
    {
        public BlogStoryRepository(BlogContext context) : base(context)
        {
        }

        public async Task<Page<BlogStory>> GetPageAsync(StoriesQuery query,
                                                        CancellationToken cancel = default)
        {
            var dataQuery = GetDataQuery(query);

            var totalCount = await dataQuery.CountAsync(cancel);

            dataQuery = GetOrderedQuery(query, dataQuery);

            var stories = await dataQuery.Skip(query.Offset)
                                         .Take(query.Limit)
                                         .ToListAsync(cancel);

            return new Page<BlogStory>
            {
                TotalCount = totalCount,
                Items = stories
            };
        }

        public Task<List<BlogStory>> GetAsync(StoriesQuery query,
                                              CancellationToken cancel = default)
        {
            var dataQuery = GetOrderedQuery(query);

            return dataQuery.Skip(query.Offset)
                            .Take(query.Limit)
                            .ToListAsync(cancel);
        }

        public Task<BlogStory> GetAsync(Guid id,
                                        CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(b => b.Id == id, cancel);
        }

        public Task<BlogStory> GetAsync(String alias,
                                        CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(b => b.Alias.Equals(alias), cancel);
        }

        public Task<BlogStory> GetWithTagsAsync(Guid id,
                                                CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .ThenInclude(x => x.Tag)
                           .FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public Task<BlogStory> GetWithTagsAsync(String alias,
                                                CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .ThenInclude(x => x.Tag)
                           .FirstOrDefaultAsync(x => x.Alias.Equals(alias), cancel);
        }

        private IQueryable<BlogStory> GetDataQuery(StoriesQuery query)
        {
            IQueryable<BlogStory> dataQuery = Entities;

            if(query.IsPublished.HasValue)
            {
                dataQuery = query.IsPublished.Value
                    ? dataQuery.Where(x => x.PublishedDate.HasValue)
                    : dataQuery.Where(x => !x.PublishedDate.HasValue);
            }

            if(query.StoriesIds.IsNotEmpty())
            {
                dataQuery = dataQuery.Where(x => query.StoriesIds.Contains(x.Id));
            }

            return dataQuery;
        }

        private IQueryable<BlogStory> GetOrderedQuery(StoriesQuery query,
                                                      IQueryable<BlogStory> dataQuery)
        {
            var orderParameters = query.OrderParameters;
            if(orderParameters.IsEmpty())
            {
                return dataQuery;
            }

            return dataQuery.ApplyOrder(orderParameters.Select(x => (new BlogStoriesOrderMapping(x.orderField).Base, x.isAsc))
                                                       .ToList());
        }

        private IQueryable<BlogStory> GetOrderedQuery(StoriesQuery query)
        {
            var dataQuery = GetDataQuery(query);
            return GetOrderedQuery(query, dataQuery);
        }
    }
}