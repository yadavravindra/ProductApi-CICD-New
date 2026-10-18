using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }
        [Range(1,1000000,ErrorMessage ="Price must be greater than zero")]
        public decimal Price { get; set; }
    }
}
