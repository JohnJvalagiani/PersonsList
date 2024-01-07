using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Abstraction
{
    public interface IRepo<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);
        void AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetById(int Id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null);

        Task Remove(int Id);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> Update(TEntity entity);




    }
}
