using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using GenRep.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Repository
{
    public class BlogStoryTagRepository : BaseRepository<BlogStoryTag>,
                                          IBlogStoryTagRepository
    {
        public BlogStoryTagRepository(BlogContext context) : base(context)
        {
        }

        public Task<List<BlogStoryTag>> GetByStoryIdAsync(Guid storyId,
                                                          CancellationToken cancel)
        {
            return Entities.Where(x => x.BlogStoryId == storyId)
                           .ToListAsync(cancel);
        }

        public Task DeleteRangeAsync(List<BlogStoryTag> tags,
                                     CancellationToken cancel)
        {
            return base.DeleteRangeAsync(tags, cancel);
        }

        public new Task<BlogStoryTag> AddAsync(BlogStoryTag blogStoryTag,
                                               CancellationToken cancel)
        {
            return base.AddAsync(blogStoryTag, cancel);
        }

        public Task AddRangeAsync(List<BlogStoryTag> blogStoryTags,
                                  CancellationToken cancel)
        {
            return base.AddRangeAsync(blogStoryTags, cancel);
        }

        public new Task DeleteRangeAsync(IEnumerable<BlogStoryTag> tags,
                                         CancellationToken cancel)
        {
            return base.DeleteRangeAsync(tags, cancel);
        }

        public new Task DeleteAsync(BlogStoryTag blogStoryTag,
                                CancellationToken cancel)
        {
            return base.DeleteAsync(blogStoryTag, cancel);
        }
    }
}