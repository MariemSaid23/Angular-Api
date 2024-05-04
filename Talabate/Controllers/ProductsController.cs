using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;
using Talabat.core.Specifications;
using Talabat.core.Specifications.Product_Specs;
using Talabate.Dtos;

namespace Talabate.Controllers
{
  
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;
        //test
        public ProductsController(IGenericRepository<Product> productsRepo,IMapper mapper)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var spec=new ProductWithBrandAndCategorySpecifications();
            var products=await _productsRepo.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDto>>(products));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec=new BaseSpecifications<Product>();
            var product =await _productsRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                return NotFound();
            }
           return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }

            
    }
}
