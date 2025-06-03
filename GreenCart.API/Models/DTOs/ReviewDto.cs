
namespace GreenCart.API.Models.DTOs
{
    public class ReviewDto
    {
        public int ProductId { get; set; }
        public int Rating { get; set; } // 1 to 5
        public string Comment { get; set; }
    }
}
