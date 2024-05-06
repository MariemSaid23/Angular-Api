using Microsoft.EntityFrameworkCore;
using Talabat.core.Entities;
using Talabat.core.Specifications;

namespace Talabat.Repositery.Generic_Repository
{
    internal static class SpesificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> Spec)
        {

            var query = inputQuery;//_dbContext.Set<TEntity>();
            if (Spec.Criteria != null) //E=>E.id ==1
            {
                query = query.Where(Spec.Criteria);
            }
            if(Spec.OrderBy != null)
            {
                query = query.OrderBy(Spec.OrderBy); 
            }
            else if (Spec.OrderByDesc != null)
            {
                query=query.OrderByDescending(Spec.OrderByDesc);
            }


            if(Spec.IsPaginationEnabled)
            {
                query = query.Skip(Spec.Skip).Take(Spec.Take);
            }
            //query=_dbContext.Set<TEntity>().Where(E=>E.id ==1); 
            //include Expressions
            //1-p=>p.Brand
            //2-p=>p.category
            query = Spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //query=_dbContext.Set<TEntity>().Where(E=>E.id ==1).Include(p=>p.Brand);//first itegration
            //query=_dbContext.Set<TEntity>().Where(E=>E.id ==1).Include(p=>p.Brand).Include(p=>p.category);//secound

            return query;

        }
    }
}
