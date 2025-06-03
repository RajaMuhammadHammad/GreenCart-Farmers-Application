using System;

namespace GreenCart.API.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; }

        // Navigation property
        public virtual Order Order { get; set; }
    }
}
