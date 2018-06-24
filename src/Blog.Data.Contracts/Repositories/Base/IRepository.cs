using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blog.Data.Contracts.Repositories.Base
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        List<T> Get(Int32 skip, Int32 top);
        T FirstOrDefault(Expression<Func<T, Boolean>> predicate);
        List<T> Where(Expression<Func<T, Boolean>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateMany(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> range);
        Boolean Any();
        Boolean Any(Expression<Func<T, Boolean>> predicate);
        Int32 Count();
        Int32 Count(Expression<Func<T, Boolean>> expression);
    }
}