using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        void Register<TEntity>(IRepository<TEntity> repository) where TEntity:class;

        void Commit();
    }
}
