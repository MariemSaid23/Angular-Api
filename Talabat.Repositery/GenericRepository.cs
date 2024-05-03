using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;
using Talabat.core.Specifications;
using Talabat.Repositery.Data;

namespace Talabat.Repositery
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()

        {
            ///if(typeof(T)==typeof(Product)) {
            ///return (IEnumerable<T>) await _dbContext.Set<Product>().Include(p=>p.Brand).Include(p=>p.Category).ToListAsync();
            ///}
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
           return await ApllySpecifications(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            ///if (typeof(T) == typeof(Product))
            ///{
            ///    return await _dbContext.Set<Product>().Where(p=>p.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;  
            ///}
                return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApllySpecifications(spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApllySpecifications(ISpecifications<T> spec) {
            return SpesificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}
