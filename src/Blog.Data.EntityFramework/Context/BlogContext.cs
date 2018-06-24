using System;
using Blog.Core.Entities;
using Blog.Data.EntityFramework.Mappings;
using Blog.Data.EntityFramework.Mappings.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Context
{
    public class BlogContext : DbContext
    {
        private readonly String _connectionString;

        public BlogContext()
        {
        }

        public BlogContext(String connectionString)
        {
            _connectionString = connectionString;
        }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<BlogStory> BlogStories { get; set; }
        public DbSet<BlogStoryTag> BlogStoryTag { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AddMapping(new BlogStoryMapping());
            builder.AddMapping(new BlogStoryTagMapping());
            builder.AddMapping(new TagMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}