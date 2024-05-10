using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core;
using Talabat.core.Entities;
using Talabat.core.Entities.Order_Aggregate;
using Talabat.core.Repositories.Contract;
using Talabat.Repositery.Data;
using Talabat.Repositery.Generic_Repository;

namespace Talabat.Repositery
{
    public class UnitOfWork : IunitOfWork
    {
        private readonly StoreContext _dbContext;

        // public Dictionary<string ,GenericRepository<BaseEntity>> _repositories;


        private Hashtable _repositories;
        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();

        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(key))
            {
                var repository =new GenericRepository<TEntity>(_dbContext);

                _repositories.Add(key, repository);

            }
            return _repositories[key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
       => await _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
     => _dbContext.DisposeAsync();


    }
}
