using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications:BaseSpecifications<Product>
    {
        private int id;

        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams) :base(
            p=>
            (!specParams.BrandId.HasValue||p.BrandId== specParams.BrandId.Value)&&
            (!specParams.CategoryId.HasValue||p.CategoryId==specParams.CategoryId.Value)
            )
        {
            /*Criteria = p => p.Id == id;*/ // Assuming Product has an Id property // Assuming Product has an Id property
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);

            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }

            ApplyPagination((specParams.PageIndex-1)*specParams.PageSize, specParams.PageSize);
        }

        public ProductWithBrandAndCategorySpecifications(int id)
        {
            this.id = id;
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
