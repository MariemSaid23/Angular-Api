using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    { 
        //for where
       public Expression<Func<T, bool>>? Criteria { get; set; } //p=>p.id

        //to include
        public List<Expression<Func<T,object>>> Includes {  get; set; }  
        //sorting
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }  
        
        public bool IsPaginationEnabled { get; set; }

    }
}
