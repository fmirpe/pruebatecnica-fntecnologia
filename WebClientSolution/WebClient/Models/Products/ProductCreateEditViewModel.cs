using System.ComponentModel.DataAnnotations;

namespace WebClient.Models.Products
{
    public class ProductCreateEditViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = default!;

        public bool IsActive { get; set; } = true;
    }
}
