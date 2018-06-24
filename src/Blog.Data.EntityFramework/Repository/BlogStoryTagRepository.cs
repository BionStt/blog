using System;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using Blog.Data.EntityFramework.Repository.Base;

namespace Blog.Data.EntityFramework.Repository
{
    public class BlogStoryTagRepository : BaseRepository<BlogStoryTag>, IBlogStoryTagRepository
    {
        public BlogStoryTagRepository(BlogContext context) : base(context) { }
    }
}