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

        public ProductWithBrandAndCategorySpecifications(string? sort, int? brandId, int? categoryId) :base(
            p=>
            (!brandId.HasValue||p.BrandId==brandId.Value)&&
            (!categoryId.HasValue||p.CategoryId==categoryId.Value)
            )
        {
            /*Criteria = p => p.Id == id;*/ // Assuming Product has an Id property // Assuming Product has an Id property
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);

            if(!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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
        }

        public ProductWithBrandAndCategorySpecifications(int id)
        {
            this.id = id;
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
