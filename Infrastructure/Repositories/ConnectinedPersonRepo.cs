using Domain.Interfaces.Repository;
using IG.Core.Data.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ConnectedPersonRepo : IConnectedPersonsRepo
    {
        protected readonly UserDbContext _context;
        protected readonly DbSet<ConnectedPerson> _set;
        public IUnitOfWork UnitOfWork => _context;
        public ConnectedPersonRepo(UserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<ConnectedPerson>();
        }

        public async Task<ConnectedPerson> AddAsync(ConnectedPerson entity)
        {
            NullChecker(entity);
            var _entity = await _set.AddAsync(entity);
            return _entity.Entity;
        }

        public async Task<IEnumerable<ConnectedPerson>> GetAllAsync() => await _set.ToListAsync();

        public async Task<ConnectedPerson> GetByIdAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();

            return await FindElement(Id);
        }

        public async Task<bool> RemoveAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();
            var entity = await FindElement(Id);
            _set.Remove(entity);
            return true;
        }

        public ConnectedPerson Update(ConnectedPerson entity)
        {
            NullChecker(entity);
            var _entity = _set.Update(entity);
            return _entity.Entity;
        }

        private async Task<ConnectedPerson> FindElement(int Id) => await _set.FindAsync(Id);

        public bool RemoveRangeAsync(IEnumerable<ConnectedPerson> entities)
        {
            NullChecker(entities);
            _set.RemoveRange(entities);
            return true;
        }

        public async Task AddRangeAsync(IEnumerable<ConnectedPerson> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<ConnectedPerson>> GetByQueryAsync(Expression<Func<ConnectedPerson, bool>> filter = null, Func<IQueryable<ConnectedPerson>,
            IOrderedQueryable<ConnectedPerson>> OrderBy = null)
        {
            IQueryable<ConnectedPerson> query = _set;
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

        private void NullChecker(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);
        }
    }
}
