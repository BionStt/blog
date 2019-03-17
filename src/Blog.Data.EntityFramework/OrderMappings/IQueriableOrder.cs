using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Core.Helpers;

namespace Blog.Data.EntityFramework.OrderMappings
{
    public static class IQueriableOrder
    {
        public static IQueryable<T> ApplyOrder<T>(this IQueryable<T> query,
                                                  List<(OrderMapping<T> map, Boolean isAsc)> orderParameters)
        {
            if(orderParameters.IsEmpty())
            {
                return query;
            }

            //first order
            var orderQuery = ApplyFirstLevel(query, orderParameters[0]);

            for (var i = 1; i < orderParameters.Count; i++)
            {
                orderQuery = ApplySecondLevel(orderQuery, orderParameters[i]);
            }

            return orderQuery;
        }

        private static IOrderedQueryable<T> ApplyFirstLevel<T>(IQueryable<T> query,
                                                               (OrderMapping<T> map, Boolean isAsc) parameter)
        {
            var orderExpression = parameter.map.ToExpression();
            return parameter.isAsc
                ? query.OrderBy(orderExpression)
                : query.OrderByDescending(orderExpression);
        }

        private static IOrderedQueryable<T> ApplySecondLevel<T>(IOrderedQueryable<T> query,
                                                                (OrderMapping<T> map, Boolean isAsc) parameter)
        {
            var orderExpression = parameter.map.ToExpression();
            return parameter.isAsc
                ? query.ThenBy(orderExpression)
                : query.ThenByDescending(orderExpression);
        }
    }
}