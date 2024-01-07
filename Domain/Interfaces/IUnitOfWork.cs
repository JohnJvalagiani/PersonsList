using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
      public Task<bool> CommitAsync();
      public void Rollback();
      public void BeginTransaction();
    }
}
