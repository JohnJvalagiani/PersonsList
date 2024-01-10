using Domain.Interfaces.Repository;
using IG.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Abstraction
{
    public interface IPersonsRepo : IRepository<Person>
    {
        Task<Person> AddAsync(Person entity);
        Task AddRangeAsync(IEnumerable<Person> entities);
        Task<Person> GetByIdAsync(int Id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<IEnumerable<Person>> GetByQueryAsync(Expression<Func<Person, bool>> predicate = null,
            Func<IQueryable<Person>, IOrderedQueryable<Person>> OrderBy = null);
        Task<bool> RemoveAsync(int Id);
        bool RemoveRangeAsync(IEnumerable<Person> entities);
        Person Update(Person entity);
    }
}
