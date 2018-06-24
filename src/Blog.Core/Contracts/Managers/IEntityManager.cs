using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core.Contracts.Managers
{
    public interface IEntityManager<T>
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="cancel">Cancellation tocken</param>
        /// <returns>Collection of entities</returns>
        Task<List<T>> GetAllAsync(CancellationToken cancel = default);
        
        /// <summary>
        /// Get entities count
        /// </summary>
        /// <param name="cancel">Cancellation tocken</param>
        /// <returns>Count of entities</returns>
        Task<Int32> CountAsync(CancellationToken cancel = default);
    }
}