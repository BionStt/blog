using Blog.Core.Entities;
using Blog.Data.EntityFramework.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.EntityFramework.Mappings
{
    public class TagMapping : EntityMapping<Tag>
    {
        public override void Map(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.Alias)
                   .HasName("TagSlug")
                   .IsUnique();
            
            builder.Property(t => t.Alias)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(t => t.Name)
                   .HasMaxLength(256)
                   .IsRequired();

            
            builder.Property(t => t.SeoTitle);
            builder.Property(t => t.SeoDescription);
        }
    }
}