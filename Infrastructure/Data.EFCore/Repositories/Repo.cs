using Core.Services.Abstraction;
using IG.Core.Data.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementation
{
    public class Repo<TEntity> : IRepo<TEntity> where TEntity : BaseEntity
    {
        protected readonly UserDbContext _context;
        protected readonly DbSet<TEntity> _set;

        public Repo(UserDbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            NullChecker(entity);

            var _entity = await _set.AddAsync(entity);

            return _entity.Entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll() => await _set.ToListAsync();


        public virtual async Task<TEntity> GetById(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();

            return await FindElement(Id);
        }

        public virtual async Task Remove(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();

            var entity = await FindElement(Id);

            _set.Remove(entity);

        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
                NullChecker(entity);

                var _entity = _set.Update(entity);

                return _entity.Entity;
        }


        protected async Task<TEntity> FindElement(int Id) => await _set.FindAsync(Id);

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            NullChecker(entities);

            _set.RemoveRange(entities);
        }

        public virtual void AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _set.AddRangeAsync(entities);
        }

        public virtual async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> OrderBy = null)
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (OrderBy != null)
            {
                return await Task.FromResult(OrderBy(query).AsEnumerable());
            }

            return query.AsEnumerable();
        }

        protected void NullChecker(object argument)
        {
            if (argument == null)
                throw new ArgumentNullException();

        }

    }
}
