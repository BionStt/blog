using Blog.Core.Entities;
using Blog.Data.EntityFramework.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.EntityFramework.Mappings
{
    public class BlogStoryMapping : EntityMapping<BlogStory>
    {
        public override void Map(EntityTypeBuilder<BlogStory> builder)
        {
            builder.ToTable("BlogStories");

            builder.HasKey(b => b.Id);

            builder.HasIndex(b => b.Alias)
                   .HasName("AliasIndex")
                   .IsUnique();

            builder.Property(b => b.Alias)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(b => b.Title)
                   .IsRequired();

            builder.Property(b => b.CreateDate)
                   .IsRequired();

            builder.Property(b => b.AccessToken)
                   .HasMaxLength(6);
        }
    }
}