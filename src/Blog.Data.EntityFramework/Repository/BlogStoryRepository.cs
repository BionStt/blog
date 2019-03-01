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

        public async Task<Page<BlogStory>> GetPageAsync(BlogStoryQuery query,
                                                        CancellationToken cancel = default)
        {
            var dataQuery = ExtendQuery(Entities, query);

            var totalCount = await dataQuery.CountAsync(cancel);

            dataQuery = OrderQuery(dataQuery, query);

            var stories = await dataQuery.Skip(query.Offset)
                                         .Take(query.Limit)
                                         .ToListAsync(cancel);

            return new Page<BlogStory>
            {
                TotalCount = totalCount,
                Items = stories
            };
        }

        public async Task<Page<BlogStory>> GetPageWithTagsAsync(BlogStoryQuery query,
                                                                CancellationToken cancel = default)
        {
            IQueryable<BlogStory> dataQuery = Entities.Include(x => x.BlogStoryTags)
                                                      .ThenInclude(x => x.Tag);

            dataQuery = ExtendQuery(dataQuery, query);

            var totalCount = await dataQuery.CountAsync(cancel);

            dataQuery = OrderQuery(dataQuery, query);

            var stories = await dataQuery.Skip(query.Offset)
                                         .Take(query.Limit)
                                         .ToListAsync(cancel);

            return new Page<BlogStory>
            {
                TotalCount = totalCount,
                Items = stories
            };
        }

        public Task<List<BlogStory>> GetAsync(BlogStoryQuery query,
                                              CancellationToken cancel = default)
        {
            var dataQuery = ExtendAndOrderQuery(Entities, query);

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

        public new Task AddAsync(BlogStory blogStory,
                                 CancellationToken cancel)
        {
            return base.AddAsync(blogStory, cancel);
        }

        public new Task UpdateAsync(BlogStory story,
                                    CancellationToken cancel)
        {
            return base.UpdateAsync(story, cancel);
        }

        public new Task DeleteAsync(BlogStory story,
                                    CancellationToken cancel)
        {
            return base.DeleteAsync(story, cancel);
        }

        private IQueryable<BlogStory> ExtendQuery(IQueryable<BlogStory> dataQuery,
                                                  BlogStoryQuery query)
        {
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

        private IQueryable<BlogStory> OrderQuery(IQueryable<BlogStory> dataQuery,
                                                 BlogStoryQuery query)
        {
            var orderParameters = query.OrderParameters;
            if(orderParameters.IsEmpty())
            {
                return dataQuery;
            }

            return dataQuery.ApplyOrder(orderParameters.Select(x => (new BlogStoriesOrderMapping(x.orderField).Base, x.isAsc))
                                                       .ToList());
        }

        private IQueryable<BlogStory> ExtendAndOrderQuery(IQueryable<BlogStory> dataQuery,
                                                          BlogStoryQuery query)
        {
            dataQuery = ExtendQuery(dataQuery, query);
            return OrderQuery(dataQuery, query);
        }
    }
}