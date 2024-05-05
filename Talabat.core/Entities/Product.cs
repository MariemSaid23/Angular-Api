using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set;}
        //[ForeignKey("ProductBrand")]
        //[ForeignKey(nameof(Product.Brand))]

        //[ForeignKey("Brand")]
        public int BrandId { get; set; } //Foregin Key Column => ProductBrand
        
        public   ProductBrand Brand { get; set; }//navigational property[One]

        //[ForeignKey("Category")]
        public int CategoryId { get; set; }
        public   ProductCategory Category { get; set; }   //navigational property[One]
    }
}
