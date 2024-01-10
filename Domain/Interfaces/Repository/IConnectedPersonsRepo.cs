using IG.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public interface IConnectedPersonsRepo : IRepository<ConnectedPerson>
    {
        Task<ConnectedPerson> AddAsync(ConnectedPerson entity);
        Task AddRangeAsync(IEnumerable<ConnectedPerson> entities);
        Task<ConnectedPerson> GetByIdAsync(int Id);
        Task<IEnumerable<ConnectedPerson>> GetAllAsync();
        Task<IEnumerable<ConnectedPerson>> GetByQueryAsync(Expression<Func<ConnectedPerson, bool>> predicate = null,
            Func<IQueryable<ConnectedPerson>, IOrderedQueryable<ConnectedPerson>> OrderBy = null);
        Task<bool> RemoveAsync(int Id);
        bool RemoveRangeAsync(IEnumerable<ConnectedPerson> entities);
        ConnectedPerson Update(ConnectedPerson entity);
    }
}
