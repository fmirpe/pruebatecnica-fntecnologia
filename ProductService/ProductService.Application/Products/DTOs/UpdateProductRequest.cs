using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Products.DTOs
{
    public class UpdateProductRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater or equal to zero.")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
