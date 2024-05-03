using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;
using Talabat.core.Specifications;
using Talabat.core.Specifications.Product_Specs;

namespace Talabate.Controllers
{
  
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;

        //test
        public ProductsController(IGenericRepository<Product> productsRepo)
        {
            _productsRepo = productsRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec=new ProductWithBrandAndCategorySpecifications();
            var products=await _productsRepo.GetAllWithSpecAsync(spec);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec=new BaseSpecifications<Product>();
            var product =await _productsRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                return NotFound();
            }
           return Ok(product);
        }

            
    }
}
