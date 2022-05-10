using Core.Interfaces;
using Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(WebDbContext context)
        {
            Context = context;
        }

        public WebDbContext Context { get; }

        private Hashtable _repositories;

        public Task<int> Complete()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories==null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), Context);
                _repositories.Add(type,repositoryInstance);
            }
            return(IGenericRepository<TEntity>) _repositories[type];
        }
    }
}
