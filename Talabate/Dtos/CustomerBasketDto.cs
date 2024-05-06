using System.ComponentModel.DataAnnotations;
using Talabat.core.Entities.Basket;

namespace Talabate.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; } = null;

        [Required]
        public List<BasketItemDto> Items { get; set; } = null;

    }
}
