using System;
using System.Linq.Expressions;
using Blog.Core.Entities;

namespace Blog.Data.EntityFramework.OrderMappings
{
    public class BlogStoriesOrderMapping : OrderMapping<BlogStory>
    {
        public BlogStoriesOrderMapping(String fieldName) : base(fieldName)
        {
        }

        public override Expression<Func<BlogStory, Object>> ToExpression()
        {
            switch (FieldName)
            {
                case "create":
                {
                    return story => story.CreateDate;
                }
                case "publish":
                {
                    return story => story.PublishedDate;
                }
                default:
                {
                    return story => story.CreateDate;
                }
            }
        }
    }
}