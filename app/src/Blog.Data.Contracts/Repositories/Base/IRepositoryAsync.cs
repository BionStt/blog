using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Data.Contracts.Repositories.Base
{
    public interface IRepositoryAsync<T>
    {
        Task<List<T>> GetAllAsync(CancellationToken cancel = default);

        Task<List<T>> GetAsync(Int32 skip,
                               Int32 top,
                               CancellationToken cancel = default);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, Boolean>> predicate,
                                    CancellationToken cancel = default);

        Task<List<T>> WhereAsync(Expression<Func<T, Boolean>> predicate,
                                 CancellationToken cancel = default);

        Task AddAsync(T entity,
                      CancellationToken cancel = default);

        Task AddRangeAsync(IEnumerable<T> entities,
                           CancellationToken cancel = default);

        Task UpdateAsync(T entity,
                         CancellationToken cancel = default);

        Task UpdateManyAsync(IEnumerable<T> entities,
                             CancellationToken cancel = default);

        Task DeleteAsync(T entity,
                         CancellationToken cancel = default);

        Task DeleteRangeAsync(IEnumerable<T> range,
                              CancellationToken cancel = default);

        Task<Boolean> AnyAsync(CancellationToken cancel = default);

        Task<Boolean> AnyAsync(Expression<Func<T, Boolean>> predicate,
                               CancellationToken cancel = default);

        Task<Int32> CountAsync(CancellationToken cancel = default);

        Task<Int32> CountAsync(Expression<Func<T, Boolean>> expression,
                               CancellationToken cancel = default);
    }
}