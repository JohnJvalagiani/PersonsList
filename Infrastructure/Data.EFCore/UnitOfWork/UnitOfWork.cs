using Domain.Interfaces.Repository;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.EFCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserDbContext dbContext;
        private IDbContextTransaction transaction;

        public UnitOfWork(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CommitAsync()
        {
            // Save changes to the database
            var success = (await dbContext.SaveChangesAsync()) > 0;

            // Commit the transaction
            transaction?.Commit();

            return success;
        }

        public void Rollback()
        {
            // Roll back changes by disposing the transaction without committing
            transaction?.Rollback();
        }

        public void BeginTransaction()
        {
            transaction = dbContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                transaction?.Dispose();
                dbContext.Dispose();
            }
        }
    }
}
