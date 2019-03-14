using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Blog.Core.Entities;

namespace Blog.Data.EntityFramework.OrderMappings
{
    public class TagOrderMapping : OrderMapping<Tag>
    {
        public TagOrderMapping(String fieldName) : base(fieldName)
        {
        }

        public override Expression<Func<Tag, Object>> ToExpression()
        {
            switch (FieldName)
            {
                case "score":
                {
                    return tag => tag.Score;
                }
                default:
                {
                    return tag => tag.Score;
                }
            }
        }
    }
}