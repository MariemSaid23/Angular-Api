using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications.Product_Specs
{
    public class ProductsWithFilterationForCountSpecifications:BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecifications(ProductSpecParams specParams)
            
            :base(
                 p =>
            (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
            (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value)
                 )
        {
            
        }
    }
}
