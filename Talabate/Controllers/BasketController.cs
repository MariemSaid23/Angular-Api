using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Talabat.core.Entities.Basket;
using Talabat.core.Repositories.Contract;
using Talabate.Errors;

namespace Talabate.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket is null ? new CustomerBasket(id) : basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            {
                var CreateOrUpdateBasket = await _basketRepository.UpdateBasketAsync(basket);
                if (CreateOrUpdateBasket is null) return BadRequest(new ApiResponse(400));
                return Ok(CreateOrUpdateBasket);

            }
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
