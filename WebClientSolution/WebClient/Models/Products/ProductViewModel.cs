namespace WebClient.Models.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
