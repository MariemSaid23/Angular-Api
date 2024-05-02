using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;

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
            var products=await _productsRepo.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product =await _productsRepo.GetAsync(id);
            if (product == null)
            {
                return NotFound();
            }
           return Ok(product);
        }

            
    }
}
