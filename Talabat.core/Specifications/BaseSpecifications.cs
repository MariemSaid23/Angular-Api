using System.Linq.Expressions;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get; set; } =  new List<Expression<Func<T, object>>>();
        public BaseSpecifications()
        {
            //Criteria =null;
           
        }
        public BaseSpecifications(Expression<Func<T, bool>>? criteriaExpression)
        {
            Criteria =criteriaExpression;
           

        }
    }
}
