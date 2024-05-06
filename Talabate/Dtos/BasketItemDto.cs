using System.ComponentModel.DataAnnotations;

namespace Talabate.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; } = null;

        [Required]
        public string PictureUrl { get; set; } = null;

        [Required]
       [Range (0.1,double.MaxValue ,ErrorMessage ="Price Must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity Must be 1 Item at least")]
        public int Quantity { get; set; }

        [Required]
        public string Category { get; set; } = null;
        [Required]

        public string Brand { get; set; } = null;





    }
}