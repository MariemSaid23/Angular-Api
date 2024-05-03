﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.Entities;

namespace Talabat.core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications:BaseSpecifications<Product>
    {

        public ProductWithBrandAndCategorySpecifications():base()
        {
            Includes.Add(p=>p.Brand);
            Includes.Add(p=>p.Category);
        }
    }
}
