using System;
using System.Diagnostics;
using Blog.Core.Entities;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using GenRep.EntityFramework;

namespace Blog.Data.EntityFramework.Repository
{
    public class BlogStoryTagRepository : BaseRepository<BlogStoryTag>, IBlogStoryTagRepository
    {
        public BlogStoryTagRepository(BlogContext context) : base(context) { }
    }
}