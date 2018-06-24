using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.EntityFramework.Mappings.Base
{
    public abstract class EntityMapping<T> where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> builder);
    }
}