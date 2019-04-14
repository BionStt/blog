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
    public class TagRepository : BaseRepository<Tag>,
                                 ITagRepository
    {
        private const Int32 NoLimit = Int32.MaxValue;
        
        public TagRepository(BlogContext context) : base(context)
        {
        }

        public async Task<Page<Tag>> GetPageAsync(TagsQuery query,
                                                  CancellationToken cancel = default)
        {
            var dataQuery = GetDataQuery(query);

            var totalCount = await dataQuery.CountAsync(cancel);

            dataQuery = GetOrderedQuery(query, dataQuery);

            var tags = await dataQuery.Skip(query.Offset)
                                      .Take(query.Limit)
                                      .ToListAsync(cancel);

            return new Page<Tag>
            {
                TotalCount = totalCount,
                PageSize = query.Limit,
                Items = tags
            };
        }

        public Task<List<Tag>> GetAsync(TagsQuery query,
                                        CancellationToken cancel = default)
        {
            var dataQuery = GetDataQuery(query);
            dataQuery = GetOrderedQuery(query, dataQuery);

            return dataQuery.ToListAsync(cancel);
        }

        public Task<List<Tag>> GetTopPublishedAsync(CancellationToken cancel = default)
        {
            return GetAsync(new TagsQuery(0, NoLimit)
                            {
                                WithScores = true,
                                IsPublished = true,
                            },
                            cancel);
        }

        private IQueryable<Tag> GetOrderedQuery(TagsQuery query,
                                                IQueryable<Tag> dataQuery)
        {
            var orderParameters = query.OrderParameters;
            if(orderParameters.IsEmpty())
            {
                return dataQuery;
            }

            return dataQuery.ApplyOrder(orderParameters.Select(x => (new TagOrderMapping(x.orderField).Base, x.isAsc)).ToList());
        }

        private IQueryable<Tag> GetDataQuery(TagsQuery query)
        {
            IQueryable<Tag> dataQuery = Entities;

            if(query.IsPublished.HasValue)
            {
                dataQuery = dataQuery.Where(x => x.IsPublished == query.IsPublished.Value);
            }

            if(query.WithScores.HasValue)
            {
                dataQuery = query.WithScores.Value
                    ? dataQuery.Where(x => x.Score > 0)
                    : dataQuery.Where(x => x.Score <= 0);
            }

            return dataQuery;
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return Entities.Where(x => x.IsPublished).ToListAsync(cancel);
        }

        public Task<Tag> GetAsync(Guid id,
                                  CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public Task<Tag> GetAsync(String alias,
                                  CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(x => x.Alias == alias, cancel);
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                                      CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .FirstOrDefaultAsync(x => x.Alias.Equals(alias), cancel);
        }

        public new Task DeleteAsync(Tag tag,
                                    CancellationToken cancel = default)
        {
            return base.DeleteAsync(tag, cancel);
        }

        public new Task UpdateAsync(Tag tag,
                                    CancellationToken cancel)
        {
            return base.UpdateAsync(tag, cancel);
        }

        public new Task<Tag> AddAsync(Tag tag,
                                      CancellationToken cancel)
        {
            return base.AddAsync(tag, cancel);
        }

        public Task<Guid> GetTagIdAsync(String alias,
                                        CancellationToken cancel)
        {
            return Entities.Where(x => x.Alias == alias)
                           .Select(x => x.Id)
                           .FirstOrDefaultAsync(cancel);
        }
    }
}