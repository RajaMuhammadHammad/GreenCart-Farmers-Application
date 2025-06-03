namespace GreenCart.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Navigation property for OrderItems
        public List<OrderItem> Items { get; set; } // This line is required
    }
}
