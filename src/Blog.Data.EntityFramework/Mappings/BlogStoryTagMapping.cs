using Blog.Core.Entities;
using Blog.Data.EntityFramework.Mappings.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.EntityFramework.Mappings
{
    public class BlogStoryTagMapping : EntityMapping<BlogStoryTag>
    {
        public override void Map(EntityTypeBuilder<BlogStoryTag> builder)
        {
            builder.HasKey(b => new {b.BlogStoryId, b.TagId});
            
            builder.HasOne(b => b.BlogStory)
                   .WithMany(t => t.BlogStoryTags)
                   .HasForeignKey(k => k.BlogStoryId);
            
            builder.HasOne(t => t.Tag)
                   .WithMany(b => b.BlogStoryTags)
                   .HasForeignKey(k => k.TagId);
        }
    }
}