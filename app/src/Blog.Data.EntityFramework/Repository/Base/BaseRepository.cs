using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Blog.Data.Contracts.Repositories.Base;
using Blog.Data.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;

namespace Blog.Data.EntityFramework.Repository.Base
{
    public abstract class BaseRepository<T> : IRepository<T>, IRepositoryAsync<T> where T : class
    {
        protected DbContext _context;
        protected DbSet<T> Entities;

        protected BaseRepository(BlogContext context)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            _context = context;
            Entities = context.Set<T>();
        }

        public List<T> GetAll()
        {
            return Entities.ToList();
        }

        public List<T> Get(Int32 skip, Int32 top)
        {
            return Entities.Skip(skip)
                           .Take(top)
                           .ToList();
        }

        public T FirstOrDefault(Expression<Func<T, Boolean>> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public List<T> Where(Expression<Func<T, Boolean>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity);
            SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.AddRange(entities);
            SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Update(entity);
            SaveChanges();
        }

        public void UpdateMany(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.UpdateRange(entities);
            SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity);
            SaveChanges();
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.RemoveRange(entities);
            SaveChanges();
        }

        public Boolean Any()
        {
            return Entities.Any();
        }

        public Boolean Any(Expression<Func<T, Boolean>> predicate)
        {
            return Entities.Any(predicate);
        }

        public Int32 Count()
        {
            return Entities.Count();
        }

        public Int32 Count(Expression<Func<T, Boolean>> expression)
        {
            return Entities.Count(expression);
        }

        public Task<List<T>> GetAllAsync(CancellationToken cancel = default)
        {
            return Entities.ToListAsync(cancel);
        }

        public Task<List<T>> GetAsync(Int32 skip, Int32 top, CancellationToken cancel = default)
        {
            return Entities.Skip(skip).Take(top).ToListAsync(cancel);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, Boolean>> predicate,
                                           CancellationToken cancel = default)
        {
            return Entities.FirstOrDefaultAsync(predicate, cancel);
        }

        public Task<List<T>> WhereAsync(Expression<Func<T, Boolean>> predicate,
                                        CancellationToken cancel = default)
        {
            return Entities.Where(predicate).ToListAsync(cancel);
        }

        public Task AddAsync(T entity, CancellationToken cancel = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity);
            return SaveChangesAsync(cancel);
        }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancel = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.AddRange(entities);
            return SaveChangesAsync(cancel);
        }

        public Task UpdateAsync(T entity, CancellationToken cancel = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Update(entity);
            return SaveChangesAsync(cancel);
        }

        public Task UpdateManyAsync(IEnumerable<T> entities, CancellationToken cancel = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.UpdateRange(entities.ToArray());
            return SaveChangesAsync(cancel);
        }

        public Task DeleteAsync(T entity, CancellationToken cancel = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity);
            return SaveChangesAsync(cancel);
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancel = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.RemoveRange(entities);
            return SaveChangesAsync(cancel);
        }

        public Task<Boolean> AnyAsync(CancellationToken cancel = default)
        {
            return Entities.AnyAsync(cancel);
        }

        public Task<Boolean> AnyAsync(Expression<Func<T, Boolean>> predicate,
                                      CancellationToken cancel = default)
        {
            return Entities.AnyAsync(predicate, cancel);
        }

        public Task<Int32> CountAsync(CancellationToken cancel = default)
        {
            return Entities.CountAsync(cancel);
        }

        public Task<Int32> CountAsync(Expression<Func<T, Boolean>> expression,
                                      CancellationToken cancel = default)
        {
            return Entities.CountAsync(expression, cancel);
        }

        protected void SaveChanges()
        {
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        protected Task SaveChangesAsync(CancellationToken cancel = default)
        {
            _context.ChangeTracker.DetectChanges();
            return _context.SaveChangesAsync(cancel);
        }
    }
}