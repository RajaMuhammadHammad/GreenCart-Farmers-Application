namespace GreenCart.API.Models.DTOs
{
    public class DeliveryDTO
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public DateTime? DeliveryDate { get; set; }  // Nullable DateTime
        public string DeliveryStatus { get; set; }
    }
}
