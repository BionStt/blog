using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using Blog.Data.EntityFramework.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Repository
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(BlogContext context) : base(context)
        {
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return Entities.Where(x=>x.IsPublished).ToListAsync(cancel);
        }

        public async Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default)
        {
            var tags = await Entities.Include(x => x.BlogStoryTags)
                                     .ToListAsync(cancel);
            
            return tags.OrderByDescending(x => x.BlogStoryTags.Count).ToList();
        }

        public Task<Tag> GetAsync(Int32 id, CancellationToken cancel = default)
        {
            return FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias, CancellationToken cancel = default)
        {
            return Entities.Include(x => x.BlogStoryTags)
                           .FirstOrDefaultAsync(x => x.Alias.Equals(alias), cancel);
        }
    }
}