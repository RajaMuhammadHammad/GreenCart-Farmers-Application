// Models/Product.cs
namespace GreenCart.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int FarmerId { get; set; }
        public User Farmer { get; set; }  // if using a User entity for Farmer


        // Nullable to avoid SqlNullValueException
        public string? CreatedBy { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
