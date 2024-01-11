using Core.Services.Abstraction;
using Domain.Models;
using IG.Core.Data.Entities;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using service.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repository
{
    public class PersonsRepository : IPersonsRepo
    {
        protected readonly UserDbContext _context;
        protected readonly DbSet<Person> _set;
        public IUnitOfWork UnitOfWork => _context;
        public PersonsRepository(UserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<Person>();

        }
        public  async Task<Person> AddAsync(Person entity)
        {
            NullChecker(entity);
            var _entity = await _set.AddAsync(entity);
            return _entity.Entity;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
               var response = await _set.ToListAsync();
                return response;
        }

        public  async Task<Person> GetByIdAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();

            return await _context.Persons
            .Include(p => p.ConnectedPerson)
            .FirstOrDefaultAsync(p => p.Id == Id);
        }
        public async Task<IEnumerable<ConnectedPersonsReport>> GetConnectedPersonsReportByType(ConnectedPersonType connectedPersonType)
        {
            var query = from person in _context.Persons
                        where person.ConnectedPerson.Any(cp => cp.PersonType == connectedPersonType)
                        select new ConnectedPersonsReport
                        {
                            PersonName = person.FirstNameENG,
                            ConnectedPersonsCount = person.ConnectedPerson.Count(cp => cp.PersonType == connectedPersonType)
                        };

            return await query.ToListAsync();
        }
        public async Task<bool> RemoveAsync(int Id)
        {
            if (Id <= 0)
                throw new ArgumentNullException();
            var entity = await FindElement(Id);
            _set.Remove(entity);
            return true;
        }

        public  Person Update(Person entity)
        {
                NullChecker(entity);
                var _entity = _set.Update(entity);
            return _entity.Entity;
        }

        private async Task<Person> FindElement(int Id) => await _set.FindAsync(Id);

        public bool RemoveRangeAsync(IEnumerable<Person> entities)
        {
             NullChecker(entities);
            _set.RemoveRange(entities);
            return true;
        }

        public async Task AddRangeAsync(IEnumerable<Person> entities)
        {
            await _set.AddRangeAsync(entities);
        }

        public  async Task<IEnumerable<Person>> GetByQueryAsync(Expression<Func<Person, bool>> filter = null, Func<IQueryable<Person>,
            IOrderedQueryable<Person>> OrderBy = null)
        {
            IQueryable<Person> query = _set;
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
            if (argument == null)
                throw new ArgumentNullException();
        }

        public async Task<bool> RemoveConnectedPersonAsync(int PersonId, int connectedPersonId)
        {
            Person person = await GetByIdAsync(PersonId);

            if (person != null)
            {
                person.ConnectedPerson.Remove(person.ConnectedPerson.FirstOrDefault(c=>c.Id == connectedPersonId));
                Update(person);
            }
            return true;
        }
    }
}
