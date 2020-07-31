using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Database.Interfaces;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class EFUnitOfWork:IUnitOfWork
    {
        private readonly DbContext dataBaseContext;

        public EFUnitOfWork(OrganizerContext dataBaseContext)
        {
            this.dataBaseContext = dataBaseContext;
        }

        public void Register<TEntity>(IRepository<TEntity> repository) where TEntity : class
        {
            (repository as EFGenericRepository<TEntity>)?.SetDbContext(dataBaseContext);
        }

        public void Commit()
        {
            dataBaseContext.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dataBaseContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
