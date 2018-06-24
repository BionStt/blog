using Microsoft.EntityFrameworkCore;

namespace Blog.Data.EntityFramework.Mappings.Base
{
    public static class EntityMappingHelper
    {
        public static void AddMapping<T>(this ModelBuilder builder, EntityMapping<T> mapping) where T : class
        {
            builder.Entity<T>(mapping.Map);
        }
    }
}