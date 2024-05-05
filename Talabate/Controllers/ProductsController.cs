using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Entities;
using Talabat.core.Repositories.Contract;
using Talabat.core.Specifications;
using Talabat.core.Specifications.Product_Specs;
using Talabate.Dtos;
using Talabate.Errors;

namespace Talabate.Controllers
{
  
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoriesRepo;
        private readonly IMapper _mapper;
        //test
        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> brandsRepo,
            IGenericRepository<ProductCategory>categoriesRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
           _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string? sort,int? brandId,int? categoryId)
        {
            var spec=new ProductWithBrandAndCategorySpecifications(sort,brandId,categoryId);
            var products=await _productsRepo.GetAllWithSpecAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products));
        }
        [ProducesResponseType (typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product =await _productsRepo.GetWithSpecAsync(spec);
            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }
           return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }

        [HttpGet("brands")] //base url //Get :/api/products/brands
                            

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands=await _brandsRepo.GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories=await _categoriesRepo.GetAllAsync(); 
            
            return Ok(categories);
        }
    }
}
