using System;
using System.Linq.Expressions;

namespace Blog.Data.EntityFramework.OrderMappings
{
    public abstract class OrderMapping<T>
    {
        protected String FieldName;

        protected OrderMapping(String fieldName)
        {
            FieldName = fieldName.ToLowerInvariant();
        }
        
        public abstract Expression<Func<T, Object>> ToExpression();
        
        public OrderMapping<T> Base => this;
    }
}